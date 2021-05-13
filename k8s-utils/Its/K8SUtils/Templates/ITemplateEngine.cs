using System.Reflection;

namespace Its.K8SUtils.Templates
{
    public interface ITemplateEngine
    {
        void RegisterTemplateAssembly(string templateName, string assemblyName);
        void RegisterTemplateString(string templateName, string content);
        void RegisterTemplateFile(string templateName, string fname);
        string RenderTemplate<T>(string templateName, T model);
        void SetAssembly(Assembly asm);
    }
}