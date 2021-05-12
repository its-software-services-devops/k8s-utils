using CommandLine;

namespace Its.K8SUtils.Options
{
    [Verb("compare", HelpText = "Compare K8S resources exported files")]
    public class CompareOptions : BaseOptions
    {
        [Option('b', "base", Required = true, HelpText = "The based file to compare")]
        public string BasedFilePath { get; set; }

        [Option('n', "new", Required = true, HelpText = "The newer resource exported file to compare")]
        public string NewFilePath { get; set; }

        [Option('o', "out", Required = true, HelpText = "The output file to save the comparison results")]
        public string OutputFilePath { get; set; }              
    }    
}
