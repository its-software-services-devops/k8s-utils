using NUnit.Framework;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public class ActionInfoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new InfoOptions();
            var act = new ActionInfo();

            act.SetProcessor(null);

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }
    }
}