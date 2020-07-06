using Elk.Core;
using System;

namespace Eagle.CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var domains = FileService.GetDomain();
            foreach (var name in Enum.GetNames(typeof(TemplateType)))
            {
                var type = (TemplateType)Enum.Parse(typeof(TemplateType), name);
                var temp = FileService.FilterDomains(domains, type);
                foreach (var item in FileService.FilterDomains(domains, type))
                {
                    switch (type)
                    {
                        case TemplateType.Controller:
                            new ControllerGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.EntityPartialView:
                            new EntityPartialViewGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.FilterPartialView:
                            new FilterPartialViewGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.ListPartialView:
                            new ListPartialViewGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.ManageView:
                            new ManageViewGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.ISrv:
                            new ISrvGenerator().Generate(item.Value, item.Key);
                            break;
                        case TemplateType.Srv:
                            new SrvGenerator().Generate(item.Value, item.Key);
                            break;
                    }
                }
            }

        }
    }
}
