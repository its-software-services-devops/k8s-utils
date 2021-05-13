using NUnit.Framework;

namespace Its.K8SUtils.Processors
{
    public class ProcessorUnitTest : BaseProcessor
    {
        public override string Do()
        {
            return "";
        }
    }

    public class BaseProcessorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetterSetterTest()
        {
            //To cover the test coverage

            ProcessorUnitTest proc = new ProcessorUnitTest();
            proc.SetExecutor(null);
            proc.SetOptions(null);
        }
    }
}