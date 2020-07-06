using System;
using System.IO;
using System.Text;
using Eagle.CodeGenerator.Templates;

namespace Eagle.CodeGenerator
{
    public class FilterPartialViewGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(ViewTemplate.Filters);
            base.Replacer(domain, template);
            foreach (var prop in domain.Props)
            {
                if (prop.IsPrimaryKey)
                    continue;
                else if (prop.IsForeignKey)
                    template.Replace("##filters##", $"<div class='col-12 col-sm-6'><custom-select-for for='{prop.Name}F' class='form-control select2 w-100 with-ajax' " +
                        $"items=\"Model.{prop.Name}==0?new List<SelectListItem>():new List<SelectListItem> {{" +
                                    "new SelectListItem{" +
                                        "Text = Model.Category.Name," +
                                        "Value = Model.CategoryId.ToString()," +
                                        "Selected = true}}\"" +
                        $" select-data-url='@Url.Action(\"Search\", \"{prop.Name.Replace("Id", "")}\")'></custom-select-for></div>" + Environment.NewLine + "##filters##");
                else if (prop.IsEnum) template.Replace("##filters##", $"<div class='col-12 col-sm-6'><custom-select-for for=\"{prop.Name}F\" items=\"EnumExtension.GetEnumElements<ContentType>().Select(x => new SelectListItem{{Text = x.DisplayName,Value = x.Name}}).ToList()\"></custom-select-for></div>" + Environment.NewLine + "##filters##");
                else if (prop.IsMultilineText) continue;
                else template.Replace("##filters##", $"<div class='col-12 col-sm-6'><custom-input-for for=\"{prop.Name}F\" /></div>" + Environment.NewLine + "##filters##");
            }
            template.Replace("##filters##", string.Empty);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name} FilterPartialView:");
            Console.WriteLine(template);
            return true;
        }
    }
}
