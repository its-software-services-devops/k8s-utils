using NUnit.Framework;
using Its.K8SUtils.Options.Test;

namespace Its.K8SUtils.Options
{
    public class CompareOptionsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("", new[] {"-b", "path.txt", "-n", "aaa.txt", "-o", "out.txt"})]
        [TestCase("", new[] {"--base", "path.txt", "--new", "aaa.txt", "--out", "out.txt"})]
        public void OptionsCompareBaseTest(string dummy, string[] args)
        {
            CompareOptions opt = TestUtils.TestParseArguments<CompareOptions>(args);
            Assert.AreEqual("path.txt", opt.BasedFilePath);
            Assert.AreEqual("out.txt", opt.OutputFilePath);
            Assert.AreEqual("aaa.txt", opt.NewFilePath);
        }
    }
}