using NUnit.Framework;

namespace Its.K8SUtils.Utils
{
    public class ExecutorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("echo", "hello world", "hello world\n")]
        [TestCase("ls", "shsdfowksik-notfound.txt", "")]
        public void RunTest(string cmd, string args, string result)
        {
            Executor exec = new Executor();
            string output = exec.Run(cmd, args);

            Assert.AreEqual(result, output);
        }
    }
}