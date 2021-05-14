using Its.K8SUtils.Utils;

namespace Its.K8SUtils.Processors.Exporters
{
    public class ResourceExporterExecutor : IExecutor
    {
        private string retStr = "";

        public string Run(string cmd, string argv)
        {
            return retStr;
        }

        public void SetReturnString(string str)
        {
            retStr = str;
        }
    } 
}
