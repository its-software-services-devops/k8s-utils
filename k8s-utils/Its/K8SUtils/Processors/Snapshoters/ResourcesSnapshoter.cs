using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Its.K8SUtils.Options;
using Serilog;

namespace Its.K8SUtils.Processors.Snapshoters
{
    public class ResourcesSnapshoter : BaseProcessor
    {
        private readonly Regex nameRegex = new Regex(@"^  name:\s+(.+)$");
        private readonly Regex kindRegex = new Regex(@"^kind:\s+(.+)$");
        private readonly Regex nsRegex = new Regex(@"^  namespace:\s+(.+)$");

        private List<string> excludedList = new List<string>() 
        {
            ".items.[].metadata.managedFields", 
            ".items.[].status",
            ".items.[].metadata.creationTimestamp",
            ".items.[].metadata.generation",
            ".items.[].metadata.selfLink",
            //".items.[].metadata.resourceVersion",
            ".items.[].metadata.uid",
            ".items.[].metadata.annotations.\\\"kubectl.kubernetes.io/last-applied-configuration\\\"",
        };

        private readonly string tmpFile = Path.GetTempFileName();
        private readonly string tmpDir = Path.GetTempPath();
        private readonly string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

        private readonly string filterCmd = "yq";
        private readonly string filterArgv = "e \"del({0})\" {1}";

        private readonly string cmdStr = "kubectl";
        private readonly string getResSubCmd = "api-resources -o name --namespaced={0}";
        private readonly string getKindNsSubCmd = "get {0} --all-namespaces --sort-by=.metadata.name -o yaml";
        private readonly string getKindGbSubCmd = "get {0} --sort-by=.metadata.name -o yaml";

        public override string Do()
        {
            var nsLevelRes = GetAvailableResources("true");
            var gbLevelRes = GetAvailableResources("false");

            var nsLvlRes = GetResources(nsLevelRes, getKindNsSubCmd);
            var glbLvlRes = GetResources(gbLevelRes, getKindGbSubCmd);

            WriteFile(glbLvlRes, nsLvlRes);
            
            return "";
        }

        private void WriteFile(string glbRes, string nsRes)
        {
            var opt = options as SnapshotOptions;

            string gbLevelName = String.Format("{0}_global.yaml", tmpFile);
            string nsLevelName = String.Format("{0}_ns.yaml", tmpFile);

            string gbFilterName = String.Format("{0}/global_resources.yaml", opt.ExportOutputDir);
            string nsFilterName = String.Format("{0}/ns_resources.yaml", opt.ExportOutputDir);

            File.WriteAllText(gbLevelName, glbRes);
            File.WriteAllText(nsLevelName, nsRes);

            string glbFilterCtn = GetFilteredContent(gbLevelName);
            string nsFilterCtn = GetFilteredContent(nsLevelName);

            int glbCnt = SaveResources(glbFilterCtn, "gb");
            int nsCnt = SaveResources(nsFilterCtn, "ns");

            Log.Information("Wrote [{0}] resources for global level, and [{1}] resources for namespace level", glbCnt, nsCnt);
        }

        private int SaveResources(string content, string mode)
        {
            var arr = Utils.Utils.StringsToArray(content);

            bool inArray = false;
            var lines = new List<string>();

            foreach (string line in arr)
            {
                string recType = line.Substring(0, 4);
                string keepLine = line.Substring(4);

                if (recType.Equals("  - "))
                {
                    if (inArray)
                    {
                        SaveResource(lines, mode);
                        lines.Clear();
                    }

                    lines.Add(keepLine);
                    inArray = true;
                }
                else if (inArray && recType.Equals("    "))
                {
                    lines.Add(keepLine);
                }
                else if (inArray)
                {
                    inArray = false;
                    
                    SaveResource(lines, mode);
                    lines.Clear();                    
                }
            }

            if (lines.Count > 0)
            {
                SaveResource(lines, mode);
            }

            return arr.Count;
        }

        private void SaveResource(List<string> lines, string mode)
        {
            string name = "";
            string nameSpace = "";
            string kind = "";

            foreach (string line in lines)
            {     
                if (nameRegex.IsMatch(line))
                {
                    var match = nameRegex.Match(line);
                    name = match.Groups[1].Value;                    
                }
                else if (nsRegex.IsMatch(line))
                {
                    var match = nsRegex.Match(line);
                    nameSpace = match.Groups[1].Value;                
                }
                else if (kindRegex.IsMatch(line))
                {
                    var match = kindRegex.Match(line);
                    kind = match.Groups[1].Value;                      
                }
            }

            SaveResourceToFile(name, kind, nameSpace, lines, mode, timeStamp);
        }

        private void SaveResourceToFile(string name, string kind, string ns, List<string> lines, string mode, string ts)
        {
            string dirName = String.Format("{0}/{1}/{2}/{3}", tmpDir, ts, ns, kind);
            if (mode.Equals("gb"))
            {
                dirName = String.Format("{0}/{1}/{2}/{3}", tmpDir, ts, "__global__", kind);
            }

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            string pathName = String.Format("{0}/{1}.yaml", dirName, name.Replace(':', '#'));
            File.WriteAllLines(pathName, lines);

            Log.Information("Saved resource to file [{0}]", pathName);
        }

        private string GetResources(List<string> kinds, string subCmd)
        {
            string allKinds = String.Join(",", kinds.ToArray());
            string argv = String.Format(subCmd, allKinds);

            string result = executor.Run(cmdStr, argv);

            return result;
        }

        private List<string> GetAvailableResources(string nsFlag)
        {
            string argv = String.Format(getResSubCmd, nsFlag);

            string result = executor.Run(cmdStr, argv);
            var arr = Utils.Utils.StringsToArray(result);

            return arr;
        }

        private string GetFilteredContent(string fName)
        {
            string excList = String.Join(',', excludedList);

            string argv = String.Format(filterArgv, excList, fName);
            string result = executor.Run(filterCmd, argv);

            return result;
        }
    }
}