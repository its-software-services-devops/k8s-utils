using System;
using System.IO;
using System.Collections.Generic;
using Its.K8SUtils.Options;
using Serilog;

namespace Its.K8SUtils.Processors.Exporters
{
    public class ResourcesExporter : BaseExporter
    {
        private readonly string cmdStr = "kubectl";
        private readonly string getResSubCmd = "api-resources -o name --namespaced={0}";
        private readonly string getKindNsSubCmd = "get {0} --all-namespaces {1} --sort-by=.metadata.namespace";
        private readonly string getKindNsSubCmdPath = "-o=jsonpath=\"{range .items[*]}{.metadata.namespace}{';'}{.kind}{';'}{.metadata.name}{'\\n\'}{end}\"";
        private readonly string getKindGbSubCmd = "get {0} {1} --sort-by=.kind";
        private readonly string getKindGbSubCmdPath = "-o=jsonpath=\"{range .items[*]}{.kind}{';'}{.metadata.name}{'\\n'}{end}\"";

        public override string Do()
        {
            var nsLevelRes = GetAvailableResources("true");
            var gbLevelRes = GetAvailableResources("false");

            var nsLvlRes = GetKinds(nsLevelRes, getKindNsSubCmd, getKindNsSubCmdPath);
            var glbLvlRes = GetKinds(gbLevelRes, getKindGbSubCmd, getKindGbSubCmdPath);

            WriteFile(glbLvlRes, nsLvlRes);
            
            return "";
        }

        private void WriteFile(List<string> glbres, List<string> nsRes)
        {
            var opt = options as ExportOptions;
            using StreamWriter file = new(opt.ExportOutputPath);

            foreach (string line in glbres)
            {
                file.WriteLine(String.Format("GB;{0}", line));
            }
            Log.Information("Wrote {0} lines to file [{1}]", glbres.Count, opt.ExportOutputPath);

            foreach (string line in nsRes)
            {
                file.WriteLine(String.Format("NS;{0}", line));
            }
            Log.Information("Wrote {0} lines to file [{1}]", nsRes.Count, opt.ExportOutputPath);
        }

        private List<string> GetKinds(List<string> kinds, string subCmd, string jsonPath)
        {
            string allKinds = String.Join(",", kinds.ToArray());

            string argv = String.Format(subCmd, allKinds, jsonPath);

            string result = executor.Run(cmdStr, argv);
            var arr = Utils.Utils.StringsToArray(result);
            arr.Sort();

            return arr;
        }

        private List<string> GetAvailableResources(string nsFlag)
        {
            string argv = String.Format(getResSubCmd, nsFlag);

            string result = executor.Run(cmdStr, argv);
            var arr = Utils.Utils.StringsToArray(result);

            return arr;
        }
    }
}