using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;
namespace Its.K8SUtils.Actions
{
    public abstract class BaseAction : IAction
    {
        private int lastRunStatus = 1;
        public abstract void SetProcessor(IProcessor proc);

        protected abstract int RunAction(BaseOptions options);

        public int Run(BaseOptions options)
        {
            lastRunStatus = RunAction(options);
            return lastRunStatus;
        }

        public int GetLastRunStatus()
        {
            return lastRunStatus;
        }
    }
}