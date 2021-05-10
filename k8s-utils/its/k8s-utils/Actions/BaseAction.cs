using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public abstract class BaseAction : IAction
    {
        private int lastRunStatus = 1;

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