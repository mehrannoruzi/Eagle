using Eagle.CodeGenerator.Templates;
using System;
using System.IO;
using System.Text;

namespace Eagle.CodeGenerator
{
    public class ISrvGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(ServiceTemplate.IService);
            base.Replacer(domain, template);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name}IService:");
            Console.WriteLine(template);
            return true;
        }
    }
}
