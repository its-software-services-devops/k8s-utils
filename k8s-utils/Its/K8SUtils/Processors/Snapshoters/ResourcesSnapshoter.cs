using System;
using System.IO;
using System.Collections.Generic;
using Its.K8SUtils.Options;
using Serilog;

namespace Its.K8SUtils.Processors.Snapshoters
{
    public class ResourcesSnapshoter : BaseProcessor
    {
        private List<string> excludedList = new List<string>() 
        { 
            ".items.[].metadata.managedFields", 
            ".items.[].status",
            ".items.[].metadata.creationTimestamp",
            ".items.[].metadata.generation",
            ".items.[].metadata.selfLink",
            ".items.[].metadata.resourceVersion",
            ".items.[].metadata.uid",
        };

        private readonly string tmpFile = Path.GetTempFileName();

        private readonly string filterCmd = "yq";
        private readonly string filterArgv = "e \"del({0})\" {1}";

        private readonly string cmdStr = "kubectl";
        private readonly string getResSubCmd = "api-resources -o name --namespaced={0}";
        private readonly string getKindNsSubCmd = "get {0} --all-namespaces --sort-by=.metadata.namespace -o yaml";
        private readonly string getKindGbSubCmd = "get {0} -o yaml";

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

            File.WriteAllText(gbFilterName, glbFilterCtn);
            File.WriteAllText(nsFilterName, nsFilterCtn);

            Log.Information("Wrote file [{0}] and [{1}]", gbFilterName, nsFilterName);
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