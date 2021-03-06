using Its.K8SUtils.Utils;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Processors
{
    public abstract class BaseProcessor : IProcessor
    {
        protected IExecutor executor = new Executor();
        protected BaseOptions options = null;

        public abstract string Do();

        public void SetExecutor(IExecutor exctr)
        {
            executor = exctr;
        }

        public void SetOptions(BaseOptions opts)
        {
            options = opts;
        }
    }
}