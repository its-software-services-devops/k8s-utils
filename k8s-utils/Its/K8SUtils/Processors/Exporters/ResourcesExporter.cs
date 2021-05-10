using Its.K8SUtils.Utils;

namespace Its.K8SUtils.Processors.Exporters
{
    public abstract class ResourcesExporter : BaseExporter
    {
        private readonly string cmdStr = "kubectl";

        public override string Do(string cmd, string argv)
        {
            return "";
        }
    }
}