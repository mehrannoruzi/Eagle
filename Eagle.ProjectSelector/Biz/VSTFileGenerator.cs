namespace Eagle.ProjectSelector
{
    using EnvDTE;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class VSTFileGenerator
    {
        private static string vsTempate =
            "<VSTemplate Version=\"3.0.0\" Type=\"Project\" xmlns=\"http://schemas.microsoft.com/developer/vstemplate/2005\">" + Environment.NewLine +
                  "<TemplateData>" + Environment.NewLine +
                    "<Name>{0}</Name>" + Environment.NewLine +
                    "<Description>&lt;Hillavas Eagle Base Project In .Net Core&gt;</Description>" + Environment.NewLine +
                    "<ProjectType>CSharp</ProjectType>" + Environment.NewLine +
                    "<ProjectSubType></ProjectSubType>" + Environment.NewLine +
                    "<SortOrder>1</SortOrder>" + Environment.NewLine +
                    "<CreateNewFolder>true</CreateNewFolder>" + Environment.NewLine +
                    "<DefaultName>{0}</DefaultName>" + Environment.NewLine +
                    "<ProvideDefaultName>true</ProvideDefaultName>" + Environment.NewLine +
                    "<LocationField>Enabled</LocationField>" + Environment.NewLine +
                    "<Hidden>true</Hidden>" + Environment.NewLine +
                    "<EnableLocationBrowseButton>true</EnableLocationBrowseButton>" + Environment.NewLine +
                    "<CreateInPlace>true</CreateInPlace>" + Environment.NewLine +
                  "</TemplateData>" + Environment.NewLine +
                  "<TemplateContent>" + Environment.NewLine +
                    "<Project TargetFileName=\"$ext_safeprojectname$.{0}.csproj\" File=\"Eagle.{0}.csproj\" ReplaceParameters=\"true\">" + Environment.NewLine +
                      "{1}" +
                    "</Project\r\n>" +
                  "</TemplateContent>" + Environment.NewLine +
                "</VSTemplate>";

        private static string mainVsTemplate =
            //"<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                "<VSTemplate Version=\"3.0.0\" Type=\"ProjectGroup\" xmlns=\"http://schemas.microsoft.com/developer/vstemplate/2005\">" + Environment.NewLine +
                  "<TemplateData>" + Environment.NewLine +
                    "<Name>Eagle Asp.Net Core Project</Name>" + Environment.NewLine +
                    "<Description>Eagle Base Project</Description>" + Environment.NewLine +
                    "<Icon>eagle.png</Icon>" + Environment.NewLine +
                    "<ProjectType>CSharp</ProjectType>" + Environment.NewLine +
                    //"<RequiredFrameworkVersion>2.0</RequiredFrameworkVersion>" + Environment.NewLine +
                    "<SortOrder>1010</SortOrder>" + Environment.NewLine +
                    "<TemplateID>64a58ed4-427f-4225-aa26-05481c4dc8bb</TemplateID>" + Environment.NewLine +
                    "<CreateNewFolder>true</CreateNewFolder>" + Environment.NewLine +
                    "<DefaultName>Eagle.Project</DefaultName>" + Environment.NewLine +
                    "<ProvideDefaultName>true</ProvideDefaultName>" + Environment.NewLine +
                    //"<CreateInPlace>true</CreateInPlace>" + Environment.NewLine +
                  "</TemplateData>" + Environment.NewLine +
                  "<TemplateContent>" + Environment.NewLine +
                    "<ProjectCollection>" + Environment.NewLine +
                     "{0}" +
                    "</ProjectCollection>" + Environment.NewLine +
                   "</TemplateContent>" + Environment.NewLine +
                   @"<WizardExtension>
                        <Assembly>Eagle.ProjectLauncher, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=4d7a128da4bbdbe9</Assembly>
                        <FullClassName>Eagle.ProjectLauncher.WizardImplementation</FullClassName>
                    </WizardExtension>
                   </VSTemplate>";

        public static string DestinationFulPath { set; get; }


        public static string SolutionFullPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, name;

        public static string Fire(string sourceDir, string destDir, bool firstTurn = true)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            else DeleteDirectory(destDir);
            var files = Directory.GetFiles(sourceDir);
            var template = string.Empty;
            var folders = Directory.GetDirectories(sourceDir);
            if (firstTurn)
                folders = folders.Where(x => new DirectoryInfo(x).Name != "bin" && new DirectoryInfo(x).Name != "obj").ToArray();
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                if (name == null) continue;
                var dest = Path.Combine(destDir, name);
                template += $"<Folder Name=\"{name}\" TargetFolderName=\"{name}\">" + Environment.NewLine + Fire(folder, dest, false) + "</Folder>" + Environment.NewLine;
            }
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (name == null) continue;
                var dest = Path.Combine(destDir, name);
                File.Copy(file, dest);
                if (new string[] { ".cs", ".csproj", ".asax", ".config", ".tt", ".cshtml", ".resx" }.Any(x => x == Path.GetExtension(file))) Replacer(dest);
                template += $"<ProjectItem ReplaceParameters=\"true\" TargetFileName=\"{name}\">{name}</ProjectItem>" + Environment.NewLine;
            }
            return template;
        }

        public static void DeleteDirectory(string directory, int inUseRetryCount = 3)
        {
            var counter = 0;
            while (counter < inUseRetryCount)
            {
                try
                {
                    if (!Directory.Exists(directory)) return;

                    var files = Directory.GetFiles(directory);
                    FileInfo fileInfo;
                    foreach (var file in files)
                    {
                        fileInfo = new FileInfo(file);
                        fileInfo.IsReadOnly = false;
                        File.Delete(file);
                    }

                    var folders = Directory.GetDirectories(directory);
                    foreach (var folder in folders)
                    {
                        var name = Path.GetFileName(folder);
                        if (name == null) continue;

                        DeleteDirectory(folder);
                    }
                    var di = new DirectoryInfo(directory);
                    di.Attributes &= ~FileAttributes.ReadOnly;
                    Directory.Delete(directory);
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(2000);
                    counter++;
                }
            }
        }

        public static void Genarate(string content, string projName)
        {
            var filePath = Path.Combine(DestinationFulPath, "t", projName, "t.vstemplate");
            File.Create(filePath).Dispose();
            using (TextWriter tw = new StreamWriter(filePath))
                tw.Write(string.Format(vsTempate, projName, content));
        }

        public static void GenarateMain(IList<ProjectModel> projs)
        {
            var filePath = Path.Combine(DestinationFulPath, "Template.vstemplate");
            var counter = 0;
            while (counter < 3)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    fileInfo.IsReadOnly = false;
                    File.WriteAllText(filePath, string.Empty);
                    var content = string.Empty;
                    foreach (var group in projs.GroupBy(x => x.SolutionFolder))
                    {
                        content += $"<SolutionFolder Name=\"{group.Key}\">" + Environment.NewLine;
                        foreach (var p in group)
                        {
                            content += $"<ProjectTemplateLink ProjectName=\"$safeprojectname$.{p.Name}\" CopyParameters=\"true\">" + Environment.NewLine +
                                            $"t\\{p.Name}\\t.vstemplate" + Environment.NewLine +
                                        "</ProjectTemplateLink>" + Environment.NewLine;
                        }
                        content += "</SolutionFolder>" + Environment.NewLine;
                    }
                    File.WriteAllText(filePath, string.Format(mainVsTemplate, content));
                    break;
                }
                catch (Exception e)
                {
                    System.Threading.Thread.Sleep(2000);
                    counter++;
                }
            }

        }

        public static void Replacer(string filePath)
        {
            string text = File.ReadAllText(filePath);
            text = text.Replace("Eagle", "$ext_safeprojectname$");
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.IsReadOnly = false;
            File.WriteAllText(filePath, text);

        }

    }
}
