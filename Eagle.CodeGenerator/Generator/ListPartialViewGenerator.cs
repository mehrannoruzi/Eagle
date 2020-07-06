using System;
using System.IO;
using System.Text;
using Eagle.CodeGenerator.Templates;

namespace Eagle.CodeGenerator
{
    public class ListPartialViewGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(ViewTemplate.List);
            base.Replacer(domain, template);
            foreach (var prop in domain.Props)
            {
                if (prop.IsPrimaryKey) continue;
                else if (prop.IsForeignKey) continue;

                else if (prop.IsEnum)
                {
                    template.Replace("##head##", $"<th>@Html.DisplayNameFor(x => x.Items[0].{prop.Name})</th>##head##");
                    template.Replace("##body##", $"<td>@item.{prop.Name}.GetDescription())</td>##body##");
                }
                else if (prop.IsMultilineText) continue;
                else if(prop.Type == "Boolean")
                {
                    template.Replace("##head##", $"<th>@Html.DisplayNameFor(x => x.Items[0].{prop.Name})</th>##head##");
                    template.Replace("##body##", $"<td>@(item.{prop.Name}?(<i class='zmdi zmdi-check color-green'></i>):(<i class='zmdi zmdi-close color-red'></i>))</td>##body##");
                }
                else
                {
                    template.Replace("##head##", $"<th>@Html.DisplayNameFor(x => x.Items[0].{prop.Name})</th>##head##");
                    template.Replace("##body##", $"<td>@item.{prop.Name}</td>##body##");
                }
            }
            template.Replace("##head##", string.Empty).Replace("##body##", string.Empty);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name} List Partial View:");
            Console.WriteLine(template);
            return true;
        }
    }
}
