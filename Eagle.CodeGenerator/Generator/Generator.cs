using System.Text;

namespace Eagle.CodeGenerator
{
    public abstract class Generator
    {
        public StringBuilder Replacer(DomainClass domain, StringBuilder template)
            => template.Replace("##ProjectName##", TemplateConfig.ProjectName)
                .Replace("##entity##", domain.Name);

        public virtual bool Generate(DomainClass domain, string path) => true;
    }
}
