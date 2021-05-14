using System.IO;
using NUnit.Framework;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Processors.Exporters
{
    public class ResourcesComparerTest
    {
        private const string content1 = @"
APIService
GlobalResource1
GlobalResource2
";

        private const string content2 = @"
NsResource1
NsResource2
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(content1)]
        [TestCase(content2)]    
        public void DoTest(string content)
        {
            var executor = new ResourceExporterExecutor();
            executor.SetReturnString(content);

            string outputPath = Path.GetTempFileName();

            var option = new ExportOptions();
            option.ExportOutputPath = outputPath;

            var exporter = new ResourcesExporter();
            exporter.SetOptions(option);
            exporter.SetExecutor(executor);
            exporter.Do();
        }           
    }
}