using Eagle.CodeGenerator.Templates;
using System;
using System.IO;
using System.Text;

namespace Eagle.CodeGenerator
{
    public class IRepoGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(RepoTemplate.IRep);
            base.Replacer(domain, template);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name} IRepo:");
            Console.WriteLine(template);
            return true;
        }
    }
}
