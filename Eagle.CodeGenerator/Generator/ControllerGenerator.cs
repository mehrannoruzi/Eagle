using System;
using System.IO;
using System.Text;
using Eagle.CodeGenerator.Templates;

namespace Eagle.CodeGenerator
{
    public class ControllerGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(ControllerTemplate.Template);
            base.Replacer(domain, template);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name}Controller:");
            Console.WriteLine(template);
            return true;
        }
    }
}
