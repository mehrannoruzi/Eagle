using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elk.Core;

namespace Eagle.CodeGenerator
{
    public class FileService
    {
        private static string Root = null;
        public static string GetRoot()
        {
            if (Root != null) return Root;
            var env = Environment.CurrentDirectory;
            var idx = env.IndexOf($"\\{TemplateConfig.ProjectName}.CodeGenerator");
            return env.Substring(0, idx);
        }
        public static Tuple<string, string> GetOutPutPath(DomainClass domain, TemplateType type)
        {
            var root = GetRoot();
            switch (type)
            {
                case TemplateType.Controller:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Controllers", $"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Controllers\\{domain.Name}Controller.cs");
                case TemplateType.ManageView:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}", $"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Manage.cshtml");
                case TemplateType.ListPartialView:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials", $"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials\\_List.cshtml");
                case TemplateType.FilterPartialView:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials", $"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials\\_Filters.cshtml");
                case TemplateType.EntityPartialView:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials", $"{root}\\{TemplateConfig.ProjectName}.Dashboard\\Views\\{domain.Name}\\Partials\\_Entity.cshtml");
                case TemplateType.IRepo:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Domain\\RepositoryInterfaces\\{domain.Schema}", $"{root}\\{TemplateConfig.ProjectName}.Domain\\RepositoryInterfaces\\{domain.Schema}\\I{domain.Name}Repo.cs");
                case TemplateType.Repo:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.EFDataAccess\\Repositories\\{domain.Schema}", $"{root}\\{TemplateConfig.ProjectName}.EFDataAccess\\Repositories\\{domain.Schema}\\I{domain.Name}Repo.cs");
                case TemplateType.ISrv:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Service\\Interfaces\\{domain.Schema}", $"{root}\\{TemplateConfig.ProjectName}.Service\\Interfaces\\{domain.Schema}\\I{domain.Name}Service.cs");
                case TemplateType.Srv:
                    return new Tuple<string, string>($"{root}\\{TemplateConfig.ProjectName}.Service\\Implements\\{domain.Schema}", $"{root}\\{TemplateConfig.ProjectName}.Service\\Implements\\{domain.Schema}\\{domain.Name}Service.cs");
                default: return null;
            }

        }
        public static IDictionary<string, DomainClass> FilterDomains(List<DomainClass> domains, TemplateType type)
        {
            var rep = new Dictionary<string, DomainClass>();
            foreach (var domain in domains)
            {
                var path = GetOutPutPath(domain, type);
                if (!File.Exists(path.Item2) && !TemplateConfig.Excludes[type].Any(x => x == domain.Name))
                {
                    FileOperation.CreateDirectory(path.Item1);
                    rep.Add(path.Item2, domain);
                }
            }
            return rep;
        }
        public static List<DomainClass> GetDomain()
        {
            var rep = new List<DomainClass>();
            var domainPath = $"{GetRoot()}\\{TemplateConfig.ProjectName}.Domain\\Entity";
            foreach (var folder in Directory.GetDirectories(domainPath))
                foreach (var e in Directory.GetFiles(folder))
                {
                    var name = new FileInfo(e).Name.Replace(".cs", string.Empty);
                    rep.Add(new DomainClass
                    {
                        Name = name,
                        Schema = new DirectoryInfo(folder).Name,
                        Props = GetProps(name)
                    });
                }
            return rep;
        }
        public static List<ClassProp> GetProps(string name)
        {
            var props = new List<ClassProp>();
            var assemblyName = $"{TemplateConfig.ProjectName}.Domain.{name}, {TemplateConfig.ProjectName}.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            var type = Type.GetType(assemblyName);
            foreach (var p in type.GetProperties())
            {
                var notMappedAttr = (NotMappedAttribute)p.GetCustomAttribute(typeof(NotMappedAttribute));
                if (notMappedAttr != null) continue;
                var fKeyAttr = (ForeignKeyAttribute)p.GetCustomAttribute(typeof(ForeignKeyAttribute));
                if (fKeyAttr != null) continue;
                var prop = new ClassProp
                {
                    Name = p.Name,
                    Type = p.PropertyType.Name,
                    IsEnum = p.PropertyType.IsEnum
                };
                var dtAttr = (DataTypeAttribute)p.GetCustomAttribute(typeof(DataTypeAttribute));
                if (dtAttr != null && dtAttr.DataType == DataType.MultilineText)
                    prop.IsMultilineText = true;
                var keyAttr = (KeyAttribute)p.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttr != null) prop.IsPrimaryKey = true;
                var t0 = (ForeignKeyAttribute)p.GetCustomAttribute(typeof(ForeignKeyAttribute));
                foreach (var fKey in type.GetProperties().Where(x => x.GetCustomAttribute(typeof(ForeignKeyAttribute)) != null).Select(x => (ForeignKeyAttribute)x.GetCustomAttribute(typeof(ForeignKeyAttribute))).ToList())
                {
                    if (fKey.Name == p.Name)
                        prop.IsForeignKey = true;
                }
                props.Add(prop);
            }
            return props;
        }
    }
}
