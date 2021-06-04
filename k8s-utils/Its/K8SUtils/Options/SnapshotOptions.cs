using CommandLine;

namespace Its.K8SUtils.Options
{
    [Verb("snapshot", HelpText = "Take snapshot all K8S resources and save to file")]
    public class SnapshotOptions : BaseOptions
    {
        [Option('o', "out", Required = true, HelpText = "Snapshot directory output path")]
        public string ExportOutputDir { get; set; }                
    }    
}
