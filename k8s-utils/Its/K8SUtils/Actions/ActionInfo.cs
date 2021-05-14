using Serilog;
using System.Reflection;
using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;

namespace Its.K8SUtils.Actions
{
    public class ActionInfo : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyVersion = assembly.GetName().Version;

            Log.Information("Version = [{0}]", assemblyVersion); 
            return 0;
        }

        public override void SetProcessor(IProcessor proc)
        {
            //Do nothing, just to implement it
        }        
    }
}