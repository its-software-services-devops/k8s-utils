using System.Collections.Generic;
using System.Reflection;
using System.IO;
using RazorEngineCore;

namespace Its.K8SUtils.Templates
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        private Assembly assembly = Assembly.GetExecutingAssembly();
        private IRazorEngine razorEngine = new RazorEngine();
        private Dictionary<string, IRazorEngineCompiledTemplate> templateCache = new Dictionary<string, IRazorEngineCompiledTemplate>();

        public RazorTemplateEngine()
        {
        }

        public void SetAssembly(Assembly asm)
        {
            assembly = asm;
        }

        public void RegisterTemplateAssembly(string templateName, string assemblyName)
        {
            string tplContent = "";
var arr = assembly.GetManifestResourceNames();
foreach (string s in arr)
{
    System.Console.WriteLine("DEBUG - [{0}]", s);
}
            using (Stream stream = assembly.GetManifestResourceStream(assemblyName))
            using (StreamReader reader = new StreamReader(stream))
            {
                tplContent = reader.ReadToEnd();
            }

            var template = razorEngine.Compile(tplContent);
            templateCache.Add(templateName, template);
        }

        public string RenderTemplate<T>(string templateName, T model)
        {
            var tpl = templateCache.GetValueOrDefault(templateName, null);
            string result = tpl.Run(model);

            return result;
        }
    }
}