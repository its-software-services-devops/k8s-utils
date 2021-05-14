using Moq;
using NUnit.Framework;
using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;

namespace Its.K8SUtils.Actions
{
    public class ActionExportTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new ExportOptions();
            var act = new ActionExport();

            var mck = new Mock<IProcessor>();
            mck.Setup(x=>x.Do()).Returns("");
            
            act.SetProcessor(mck.Object);

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }              
    }
}