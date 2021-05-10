using Its.K8SUtils.Utils;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Processors.Exporters
{
    public interface IExporter
    {
        string Do(string cmd, string argv);
        void SetExecutor(IExecutor exctr);
        void SetOptions(BaseOptions options);
    }
}