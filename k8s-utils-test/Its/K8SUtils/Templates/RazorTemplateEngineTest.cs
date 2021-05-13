using NUnit.Framework;
using System.IO;

namespace Its.K8SUtils.Templates
{
    public class TestModel
    {
        public string Name { get; set; }
    }

    public class RazorTemplateEngineTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("{@Model.Name}", "Seubpong", "{Seubpong}")]
        public void RenderTemplateTest(string template, string value, string result)
        {
            string tplPath = Path.GetTempFileName();
            File.WriteAllText(tplPath, template);

            string name = "Template1";

            TestModel model = new TestModel();
            model.Name = value;

            RazorTemplateEngine engine = new RazorTemplateEngine();
            engine.RegisterTemplateFile(name, tplPath);
            string content = engine.RenderTemplate<TestModel>(name, model);

            Assert.AreEqual(result, content);
        }
    }
}