using Moq;
using NUnit.Framework;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public class UtilsActionTest
    {
        public delegate void ActionFunc(BaseOptions opt);

        [SetUp]
        public void Setup()
        {
        }

        private int RunAction(ActionType type, ActionFunc func, int valueNeed)
        {
            var act = new Mock<IAction>();
            act.Setup(x=>x.Run(It.IsAny<BaseOptions>())).Returns(valueNeed);
            act.Setup(x=>x.GetLastRunStatus()).Returns(valueNeed);

            var opt = new Mock<BaseOptions>();

            UtilsAction.SetAction(type, act.Object);
            func(opt.Object);

            return act.Object.GetLastRunStatus();
        }

        [Test]
        public void RunExportActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Export, UtilsAction.RunExportAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        }  

        [Test]
        public void RunCompareActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Compare, UtilsAction.RunCompareAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        } 

        [Test]
        public void RunInfoActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Info, UtilsAction.RunInfoAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        }                       
    }
}