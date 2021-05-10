using Serilog;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public class ActionExport : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Log.Information("Action = [Export] Verbose = [{0}]", options.Verbosity); 
            return 0;
        }
    }
}