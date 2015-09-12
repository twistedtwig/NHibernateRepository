using System;
using System.Linq;
using System.Reflection;
using DatabaseManagement.Migrations;
using Microsoft.Build.Evaluation;
using System.IO;
using NHibernateRepo.Repos;

namespace DatabaseManagement
{
    internal class ProjectFileHandler
    {
        internal void AddFile(string projectPath, string folderName, string fileLocation)
        {
            var project = GetProject(projectPath);
            AddFolderToProject(project, projectPath, folderName);

            project.AddItem("Compile", fileLocation);
            
            project.Save();
        }

        private void AddFolderToProject(Project project, string projectPath, string name)
        {
            var loc = Path.Combine(projectPath.Substring(0, projectPath.LastIndexOf("\\")), name);
            project.AddItem("Folder", loc);            
        }

        internal Assembly LoadProject(string projectPath)
        {
            var project = GetProject(projectPath);
            project.Build();
            
            var outputPath = project.GetPropertyValue("OutputPath");
            var name = project.GetPropertyValue("AssemblyName");
            var fullPath = Path.Combine(project.DirectoryPath, outputPath);

            var path = Path.Combine(fullPath, name + ".dll");
            var projectAssembly = Assembly.LoadFile(path);
            
            return projectAssembly;
        }

        internal string RootNameSpace(string projectPath)
        {
            var project = GetProject(projectPath);
            var projectNamespace = project.GetPropertyValue("RootNamespace");
            return projectNamespace;
        }


//        internal RepoSearchResult FindRepoName(string projectPath, string repoName)
//        {
//            var loadedProject = new ProjectFileHandler().LoadProject(projectPath);
//
//            var reposFound = loadedProject
//                .GetTypes()
//                .Where(t =>
//                    t.IsSubclassOf(typeof(BaseRepo))
//                    && !t.IsGenericType
//                )
//                .ToList();
//
//            if (!reposFound.Any())
//            {
//                Console.WriteLine("No repository objects found, ensure you are pointing at the correct project.");
//                return null;
//            }
//
//            if (!string.IsNullOrWhiteSpace(repoName))
//            {
//                Console.WriteLine("Repo name was given '{0}', searching....", repoName);
//
//                var repo = reposFound.SingleOrDefault(r => r.Name == repoName);
//                if (repo == null)
//                {
//                    Console.WriteLine("Repo name was given '{0}' but none were found that matched.", repoName);
//                    return null;
//                }
//
//                Console.WriteLine("Repo '{0}' found.", repoName);
//                return new RepoSearchResult(loadedProject, repo);
//            }
//
//            if (reposFound.Count > 1)
//            {
//                Console.WriteLine("Multiple repository objects found, please specify the repo name.");
//                return null;
//            }
//
//            return new RepoSearchResult(loadedProject, reposFound.Single());
//        }


        private Project GetProject(string projectPath)
        {
            var project = ProjectCollection.GlobalProjectCollection.LoadedProjects.FirstOrDefault(p => p.ProjectFileLocation.LocationString == projectPath);
            if (project != null)
            {
                return project;
            }
            
            return new Project(projectPath);
        }

        internal void FinishedWithProject(string projectPath)
        {
            var project = ProjectCollection.GlobalProjectCollection.LoadedProjects.FirstOrDefault(p => p.ProjectFileLocation.LocationString == projectPath);
            if (project != null)
            {
                ProjectCollection.GlobalProjectCollection.UnloadProject(project);
            }
        }
    }
}
