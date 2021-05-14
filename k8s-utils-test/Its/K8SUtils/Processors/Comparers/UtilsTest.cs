using NUnit.Framework;
using System.Collections.Generic;

namespace Its.K8SUtils.Processors.Comparers
{
    public class UtilsTest
    {
        private const string content1 = @"
00_GB;AAAAA;1;1;DDDDD
00_GB;AAAAA;2;3;DDDDD
00_NS;AAAAA;BBBBB;2;3;EEEE
01_GB;AAAAA;BBBBB;CCCCC
01_NS;AAAAA;BBBBB;CCCCC;DDDDD
01_NS;AAAAA;BBBBB;CCCCC;DDDDD
01_NS;AAAAA;BBBBB;CCCCC;DDDDD
";

        private const string content2 = @"
00_GB;AAAAA;1;1;DDDDD
00_GB;AAAAA;2;3;DDDDD
00_NS;AAAAA;BBBBB;2;3;EEEE
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(content1, 2, 1, 1, 3)]
        [TestCase(content2, 2, 1, 0, 0)]
        public void CreateReconcileModelTest(string content, int cnt1, int cnt2, int cnt3, int cnt4)
        {
            var arr = Its.K8SUtils.Utils.Utils.StringsToArray(content);
            var model = Utils.CreateReconcileModel(arr);


            Assert.AreEqual(cnt1, model.SummaryGlobalRes.Count);
            Assert.AreEqual(cnt2, model.SummaryNsRes.Count);
            Assert.AreEqual(cnt3, model.DetailGlobalRes.Count);
            Assert.AreEqual(cnt4, model.DetailNsRes.Count);
        }        

        [TestCase(true, true, "BOTH")]
        [TestCase(true, false, "OLD")]
        [TestCase(false, true, "NEW")]
        [TestCase(false, false, "ERROR")]
        public void FlagToStringTest(bool oldFlag, bool newFlag, string expValue)
        {
            var result = Utils.FlagToString(oldFlag, newFlag);
            Assert.AreEqual(expValue, result);
        }

        [TestCase("NS;123456;789ABCDE", "NS;123456;789ABCDE")]
        [TestCase("GB;123456;789ABCDE", "GB;123456")]
        public void GetStatKeyTest(string line, string expValue)
        {
            var result = Utils.GetStatKey(line);
            Assert.AreEqual(expValue, result);
        }

        [TestCase("NS;123456;789ABCDE", new string[] {"789ABCDE"}, true)]
        [TestCase("NS;123456;789ABCDE", new string[] {}, false)]
        [TestCase("NS;123456;789ABCDE", new string[] {"AAAAAAAA"}, false)]
        [TestCase("GB;123456;789ABCDE", new string[] {"789ABCDE"}, false)]
        public void IsExcludedTest(string line, string[] list, bool expValue)
        {
            var arr = new List<string>();
            arr.AddRange(list);

            var result = Utils.IsExcluded(line, arr);
            Assert.AreEqual(expValue, result);
        }               
    }
}