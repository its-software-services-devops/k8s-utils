using System.Reflection;

namespace Its.K8SUtils.Templates
{
    public interface ITemplateEngine
    {
        void RegisterTemplateAssembly(string templateName, string assemblyName);
        string RenderTemplate<T>(string templateName, T model);
        void SetAssembly(Assembly asm);
    }
}