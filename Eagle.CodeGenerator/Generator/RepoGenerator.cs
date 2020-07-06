using System;
using System.IO;
using System.Text;
using Eagle.CodeGenerator.Templates;

namespace Eagle.CodeGenerator
{
    public class RepoGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(RepoTemplate.Rep);
            base.Replacer(domain, template);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name} Repo:");
            Console.WriteLine(template);
            return true;
        }
    }
}
