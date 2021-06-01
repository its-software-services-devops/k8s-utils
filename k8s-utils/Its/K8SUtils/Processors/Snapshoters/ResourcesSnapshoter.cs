using System;
using System.IO;
using System.Collections.Generic;
using Its.K8SUtils.Options;
using Serilog;

namespace Its.K8SUtils.Processors.Snapshoters
{
    public class ResourcesSnapshoter : BaseProcessor
    {
        private readonly string tmpFile = Path.GetTempFileName();

        private readonly string cmdStr = "kubectl";
        private readonly string getResSubCmd = "api-resources -o name --namespaced={0}";
        private readonly string getKindNsSubCmd = "get {0} --all-namespaces {1} --sort-by=.metadata.namespace";
        private readonly string getKindNsSubCmdPath = "-o=yaml";
        private readonly string getKindGbSubCmd = "get {0} {1}";
        private readonly string getKindGbSubCmdPath = "-o=yaml";

        public override string Do()
        {
            var nsLevelRes = GetAvailableResources("true");
            var gbLevelRes = GetAvailableResources("false");

            var nsLvlRes = GetResources(nsLevelRes, getKindNsSubCmd, getKindNsSubCmdPath);
            var glbLvlRes = GetResources(gbLevelRes, getKindGbSubCmd, getKindGbSubCmdPath);

            WriteFile(glbLvlRes, nsLvlRes);
            
            return "";
        }

        private void WriteFile(string glbRes, string nsRes)
        {   
            string gbLevelName = String.Format("{0}_global.yaml", tmpFile);
            string nsLevelName = String.Format("{0}_ns.yaml", tmpFile);

            //var opt = options as SnapshotOptions;

            using StreamWriter gbFile = new(gbLevelName);
            gbFile.WriteLine(glbRes);

            using StreamWriter nsFile = new(nsLevelName);
            nsFile.WriteLine(nsRes);

            Log.Information("Wrote file [{0}] and [{1}]", gbLevelName, nsLevelName);
        }

        private string GetResources(List<string> kinds, string subCmd, string jsonPath)
        {
            string allKinds = String.Join(",", kinds.ToArray());
            string argv = String.Format(subCmd, allKinds, jsonPath);

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
    }
}