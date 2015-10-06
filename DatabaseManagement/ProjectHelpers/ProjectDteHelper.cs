using System;
using System.Collections.Generic;
using DatabaseManagement.EnvDte;
using EnvDTE;
using System.IO;

namespace DatabaseManagement.ProjectHelpers
{
    /// <summary>
    /// Helper to load the EnvDTE object and manage projects
    /// </summary>
    internal class ProjectDteHelper
    {
        /// <summary>
        /// Loads the EnvDTE project object from the file path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Project GetProject(string path)
        {
            var projects = Projects();
            foreach (Project project in projects)
            {
                if (project.FullName.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    return project;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds files and the folders required to the project in Visual studio.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="folderName"></param>
        /// <param name="fileLocation"></param>
        /// <param name="showFile"></param>
        internal void AddFile(string projectPath, string folderName, string fileLocation, bool showFile = false)
        {
            var project = GetProject(projectPath);
            AddFolderToProject(project, projectPath, folderName);

            project.Save();

            if (showFile)
            {
                OpenFile(fileLocation);
            }
        }

        /// <summary>
        /// Opens the given file (which needs to be in Visual Studio).
        /// </summary>
        /// <param name="path"></param>
        internal static void OpenFile(string path)
        {
            var dte = DteHelper.GetDTE;
            dte.Documents.Open(path);
        }

        private static IEnumerable<Project> Projects()
        {
            Projects projects = DteHelper.GetDTE.Solution.Projects;
            var list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == ProjKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            var list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }

        
        private void AddFolderToProject(Project project, string projectPath, string name)
        {
            var loc = Path.Combine(projectPath.Substring(0, projectPath.LastIndexOf("\\")), name);

            var folders = name.Split(new [] { '\\'});

            ProjectItem currentProjectItem = null;
            foreach (var folder in folders)
            {
                var iterator = project.ProjectItems.GetEnumerator();
                while (iterator.MoveNext())
                {
                    var item = iterator.Current as ProjectItem;
                    if (item != null && item.Name == folder)
                    {
                        //found that folder, now look through that one to see if we can find the next level
                        currentProjectItem = item;
                        break;
                    }
                }                
            }

            var projItems = currentProjectItem != null ? currentProjectItem.ProjectItems : project.ProjectItems;
            projItems.AddFromDirectory(loc);           
        }
    }
}
