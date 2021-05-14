using Moq;
using NUnit.Framework;
using Its.K8SUtils.Options;
using Its.K8SUtils.Actions;

namespace Its.K8SUtils
{
    public class ProgramTest
    {
        public delegate void ActionFunc(BaseOptions opt);

        [SetUp]
        public void Setup()
        {
        }

        private IAction CreateMockedAction(int valueNeed)
        {
            var act = new Mock<IAction>();
            act.Setup(x=>x.Run(It.IsAny<BaseOptions>())).Returns(valueNeed);
            act.Setup(x=>x.GetLastRunStatus()).Returns(valueNeed);

            return act.Object;
        }

        [Test]
        public void MainTest()
        {
            int infoValueNeed = 9999;
            IAction infoAct = CreateMockedAction(infoValueNeed);
            
            int compareValueNeed = 9998;
            IAction compareAct = CreateMockedAction(compareValueNeed);

            int exportValueNeed = 9997;
            IAction exportAct = CreateMockedAction(exportValueNeed);

            UtilsAction.SetAction(ActionType.Info, infoAct);
            UtilsAction.SetAction(ActionType.Compare, compareAct);
            UtilsAction.SetAction(ActionType.Export, exportAct);

            string[] args = {"info", "-v", "hello"}; 
            Program.Main(args);

            Assert.AreEqual(infoValueNeed, infoAct.GetLastRunStatus(), "Last run status not match!!!");
            Assert.AreEqual(compareValueNeed, compareAct.GetLastRunStatus(), "Last run status not match!!!");
            Assert.AreEqual(exportValueNeed, exportAct.GetLastRunStatus(), "Last run status not match!!!");
        }            
    }
}