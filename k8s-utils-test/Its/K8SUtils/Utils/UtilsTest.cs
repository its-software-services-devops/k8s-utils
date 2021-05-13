using NUnit.Framework;

namespace Its.K8SUtils.Utils
{
    public class UtilsTest
    {
        private const string str1 = @"
abcdefgh
ijklmnop
qrstuv
wxyz
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(str1, "abcdefgh", "wxyz")]
        public void StringToArrayTest(string multiLineStr, string first, string last)
        {
            var arr = Utils.StringsToArray(multiLineStr);

            Assert.AreEqual(first, arr[0]);
            Assert.AreEqual(last, arr[arr.Count-1]);
        }
    }
}