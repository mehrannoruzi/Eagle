using System;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Eagle.ProjectSelector
{
    public static class SolutionProjects
    {
        public static string[] included = new string[] { 
            "Eagle.Service",
            "Eagle.DataAccess.Ef",
            "Eagle.DependencyResolver",
            "Eagle.Domain",
            "Eagle.Infrustructure" };
        public static string[] excluded = new string[] { 
            "Eagle.ProjectSelector",
            "Eagle.Template",
            "Eagle.ProjectLauncher"
        };


        public static IList<ProjectModel> GetProjects()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var sln = GetDTE().Solution;
            var projects = sln.Projects;
            var rep = new List<ProjectModel>();
            var item = projects.GetEnumerator();
            var idx = 0;
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                    continue;
                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    var items = GetSolutionFolderProjects(project).ToList();
                    var t = items[0].Name;
                    rep.AddRange(
                    GetSolutionFolderProjects(project).Where(x => !excluded.Any(p => p == x.Name)).Select(x => new ProjectModel
                    {
                        Index = ++idx,
                        SolutionFolder = project.Name,
                        //Name = x.Name.Split(new char[] { '.' }).Reverse().First(),
                        Name = x.Name.Replace("Eagle.",""),
                        Path = Directory.GetParent(x.FullName).FullName,
                        Selected = included.Any(p => p == x.Name)
                    }));
                }

            }
            return rep;
        }

        public static DTE2 GetDTE()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    bool isVisualStudio = displayName.StartsWith($"!VisualStudio.DTE.16.0:");
                    if (isVisualStudio)
                    {
                        object obj;
                        rot.GetObject(moniker[0], out obj);
                        var dte = obj as DTE2;
                        return dte;
                    }
                }
            }
            return null;
        }


        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                    continue;

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(subProject));
                else
                    list.Add(subProject);
            }
            return list;
        }
    }
}
