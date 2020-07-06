using System;
using System.Collections.Generic;
using System.Text;

namespace Eagle.CodeGenerator
{
    public static class TemplateConfig
    {
        public static string ProjectName = "Eagle";

        public static IDictionary<TemplateType, List<string>> Excludes = new Dictionary<TemplateType, List<string>> {
            {TemplateType.Controller,new List<string>{"Attachment", "ActionInRole" } },
            {TemplateType.ManageView,new List<string>{"Attachment", "ActionInRole", "UserInRole" } },
            {TemplateType.EntityPartialView,new List<string>{"Attachment", "ActionInRole", "UserInRole" } },
            {TemplateType.FilterPartialView,new List<string>{"Attachment", "ActionInRole", "UserInRole" } },
            {TemplateType.ListPartialView,new List<string>{"Attachment", "ActionInRole", "UserInRole" } },
            {TemplateType.IRepo,new List<string>{"Role","Action","ActionInRole","UserInRole", "Attachment" } },
            {TemplateType.Repo,new List<string>{"Attachment" } },
            {TemplateType.ISrv,new List<string>{"Attachment" } },
            {TemplateType.Srv,new List<string>{"Attachment" } }
        };
    }
}
