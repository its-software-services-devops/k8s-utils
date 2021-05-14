namespace Its.K8SUtils.Templates
{
    public interface ITemplateEngine
    {
        void RegisterTemplateString(string templateName, string content);
        void RegisterTemplateFile(string templateName, string fname);
        string RenderTemplate<T>(string templateName, T model);
    }
}