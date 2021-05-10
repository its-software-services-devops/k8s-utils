using Serilog;
using Its.K8SUtils.Options;
using Its.K8SUtils.Processors.Exporters;
namespace Its.K8SUtils.Actions
{
    public class ActionExport : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            var exp = new ResourcesExporter();
            exp.SetOptions(options);
            exp.Do();

            return 0;
        }
    }
}