using Moq;
using NUnit.Framework;
using Its.K8SUtils.Options;
using Its.K8SUtils.Processors;

namespace Its.K8SUtils.Actions
{
    public class ActionSnapshotTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new SnapshotOptions();
            var act = new ActionSnapshot();

            var mck = new Mock<IProcessor>();
            mck.Setup(x=>x.Do()).Returns("");

            act.SetProcessor(mck.Object);

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }
    }
}