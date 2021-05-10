using CommandLine;

namespace Its.K8SUtils.Options
{
    [Verb("export", HelpText = " Export K8S current state to file")]
    public class ExportOptions : BaseOptions
    {
    }    
}

