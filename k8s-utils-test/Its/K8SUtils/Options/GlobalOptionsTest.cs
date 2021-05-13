using System;
using CommandLine;
using NUnit.Framework;

namespace Its.K8SUtils.Options
{
    public class GlobalOptionsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        private T TestParseArguments<T>(string[] args)
        {
            T opt = default(T);

            var result = Parser.Default.ParseArguments<T>(args)
                .WithParsed<T>(o => {
                    opt = o;
                }); 

            return opt;
        }

        private string[] GetArgs(string verb, string type)
        {
            string[] argsLong = {verb, "--verbosity=log"};
            string[] argsShort = {verb, "-v", "log"};
            string[] argsShortNoSp = {verb, "-vlog"};

            if (type.Equals("long"))
            {
                return argsLong;
            }
            
            if (type.Equals("shortnosp"))
            {
                return argsShortNoSp;
            }

            return argsShort;
        }

        private void CheckFields(BaseOptions opt)
        {
            Assert.IsNotNull(opt, "Null returned!!!");
            Assert.AreEqual("log", opt.Verbosity, "Verbosity not match!!!");            
        }

        [TestCase("long")]
        [TestCase("short")]
        [TestCase("shortnosp")]
        public void GlobalOptionsInfoTest(string type)
        {
            string[] args = GetArgs("info", type);
            InfoOptions opt = TestParseArguments<InfoOptions>(args);

            CheckFields(opt);
        }
    }
}