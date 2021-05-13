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

            using (Stream stream = assembly.GetManifestResourceStream(assemblyName))
            using (StreamReader reader = new StreamReader(stream))
            {
                tplContent = reader.ReadToEnd();
            }

            RegisterTemplateString(templateName, tplContent);
        }

        public void RegisterTemplateFile(string templateName, string fname)
        {
            string tplContent = File.ReadAllText(fname);
            RegisterTemplateString(templateName, tplContent);
        }

        public void RegisterTemplateString(string templateName, string content)
        {
            var template = razorEngine.Compile(content);
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