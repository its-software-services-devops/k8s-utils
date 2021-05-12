using Its.K8SUtils.Options;
using Its.K8SUtils.Processors.Comparers;

namespace Its.K8SUtils.Actions
{
    public class ActionCompare : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            var exp = new ResourcesComparer();
            exp.SetOptions(options);
            exp.Do();

            return 0;
        }
    }
}