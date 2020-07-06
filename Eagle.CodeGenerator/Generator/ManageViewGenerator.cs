using Eagle.CodeGenerator.Templates;
using System;
using System.IO;
using System.Text;

namespace Eagle.CodeGenerator
{
    public class ManageViewGenerator : Generator
    {
        public override bool Generate(DomainClass domain, string path)
        {
            var template = new StringBuilder(ViewTemplate.Manage);
            base.Replacer(domain, template);
            foreach (var prop in domain.Props)
            {
                if (prop.IsPrimaryKey)
                    template.Replace("##fields##", $"@Html.HiddenFor(x => x.{domain.Name}Id)##fields##");
                else if (prop.IsForeignKey)
                    template.Replace("##fields##", $"<div class='col-12 col-sm-6'><custom-select-for for='{prop.Name}' class='form-control select2 w-100 with-ajax' " +
                        $"items=\"Model.{prop.Name}==0?new List<SelectListItem>():new List<SelectListItem> {{" +
                                    "new SelectListItem{" +
                                        "Text = Model.Category.Name," +
                                        "Value = Model.CategoryId.ToString()," +
                                        "Selected = true}}\"" +
                        $" select-data-url='@Url.Action(\"Search\", \"{prop.Name.Replace("Id", "")}\")'></custom-select-for></div>##fields##");
                else if (prop.IsEnum) template.Replace("##fields##", $"<div class='col-12 col-sm-6'><custom-select-for for=\"{prop.Name}\" items=\"EnumExtension.GetEnumElements<ContentType>().Select(x => new SelectListItem{{Text = x.DisplayName,Value = x.Name}}).ToList()\"></custom-select-for></div>##fields##");
                else if(prop.IsMultilineText) template.Replace("##fields##", $"<div class='col-12 col-sm-6'><custom-textarea-for for=\"Body\"></custom-textarea-for></div>##fields##");
                else template.Replace("##fields##", $"<div class='col-12 col-sm-6'><custom-input-for for=\"{prop.Name}\" /></div>##fields##");
            }
            template.Replace("##fields##", string.Empty);
            File.WriteAllText(path, template.ToString());
            Console.WriteLine($"----->{domain.Name} ManagelView:");
            Console.WriteLine(template);
            return true;
        }
    }
}
