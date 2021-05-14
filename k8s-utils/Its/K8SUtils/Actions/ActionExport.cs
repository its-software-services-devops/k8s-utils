using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;
using Its.K8SUtils.Processors.Exporters;

namespace Its.K8SUtils.Actions
{
    public class ActionExport : BaseAction
    {
        private IProcessor exporter = new ResourcesExporter();

        protected override int RunAction(BaseOptions options)
        {
            exporter.SetOptions(options);
            exporter.Do();

            return 0;
        }

        public override void SetProcessor(IProcessor proc)
        {
            exporter = proc;
        }
    }
}