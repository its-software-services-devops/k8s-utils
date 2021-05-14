using Its.K8SUtils.Options;
using Its.K8SUtils.Processors.Comparers;
using Its.K8SUtils.Processors;

namespace Its.K8SUtils.Actions
{
    public class ActionCompare : BaseAction
    {
        private IProcessor exporter = new ResourcesComparer();

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