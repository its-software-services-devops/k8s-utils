using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;
using Its.K8SUtils.Processors.Snapshoters;

namespace Its.K8SUtils.Actions
{
    public class ActionSnapshot : BaseAction
    {
        private IProcessor exporter = new ResourcesSnapshoter();

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