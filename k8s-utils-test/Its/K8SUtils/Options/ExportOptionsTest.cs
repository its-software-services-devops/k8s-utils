using NUnit.Framework;
using Its.K8SUtils.Options.Test;

namespace Its.K8SUtils.Options
{
    public class ExportOptionsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("path.txt", new[] {"-o", "path.txt"})]
        [TestCase("path.txt", new[] {"--out", "path.txt"})]
        public void OptionsExportOutputTest(string expected, string[] args)
        {
            ExportOptions opt = TestUtils.TestParseArguments<ExportOptions>(args);
            Assert.AreEqual(expected, opt.ExportOutputPath);
        }
    }
}