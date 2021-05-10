using CommandLine;

namespace Its.K8SUtils.Options
{
    [Verb("export", HelpText = " Export K8S current state to file")]
    public class ExportOptions : BaseOptions
    {
        [Option('o', "out", Required = true, HelpText = "Exported file output path")]
        public string ExportOutputPath { get; set; }                
    }    
}
