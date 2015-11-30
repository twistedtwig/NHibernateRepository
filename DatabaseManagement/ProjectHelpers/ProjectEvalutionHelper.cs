using DatabaseManagement.Logging;
using Microsoft.Build.Evaluation;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatabaseManagement.ProjectHelpers
{
    /// <summary>
    /// Helper to load MS Build Evaulation projects.
    /// </summary>
    internal class ProjectEvalutionHelper
    {
        /// <summary>
        /// Finds and builds project and returns the built assembly.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        internal Assembly LoadEvalutionProject(string projectPath)
        {
            LoggerBase.Log("Loading project file: " + projectPath, isDebugMessage: true);
            var project = GetEvalutionProject(projectPath);
            LoggerBase.Log("Building project file", isDebugMessage: true);
//            project.SetGlobalProperty("Configuration", "Debug");
            project.Build(new MsBuildLogger());

            var outputPath = project.GetPropertyValue("OutputPath");
            LoggerBase.Log("Project file outputpath: " + (!string.IsNullOrWhiteSpace(outputPath) ? outputPath : "output path was EMPTY -- this might cause an error"), isDebugMessage: true);
            
            var name = project.GetPropertyValue("AssemblyName");
            LoggerBase.Log("Project file assembly name: " + name, isDebugMessage: true);
            
            var fullPath = Path.Combine(project.DirectoryPath, outputPath);
            var path = Path.Combine(fullPath, name + ".dll");
            LoggerBase.Log("Loading DLL: " + path, isDebugMessage: true); 
            
            var projectAssembly = Assembly.LoadFile(path);

            LoggerBase.Log("", isDebugMessage: true); 
            return projectAssembly;
        }

        /// <summary>
        /// Finds the root namespace for the given project.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        internal string RootNameSpace(string projectPath)
        {
            var project = GetEvalutionProject(projectPath);
            var projectNamespace = project.GetPropertyValue("RootNamespace");
            return projectNamespace;
        }

        /// <summary>
        /// Clears out MS Build Evaluation Project cache information.
        /// </summary>
        /// <param name="projectPath"></param>
       internal static void FinishedWithProject(string projectPath)
        {
            var project = ProjectCollection.GlobalProjectCollection.LoadedProjects.FirstOrDefault(p => p.ProjectFileLocation.LocationString == projectPath);
            if (project != null)
            {
                ProjectCollection.GlobalProjectCollection.UnloadProject(project);
            }
        }

       private Project GetEvalutionProject(string projectPath)
       {
           var project = ProjectCollection.GlobalProjectCollection.LoadedProjects.FirstOrDefault(p => p.ProjectFileLocation.LocationString == projectPath);
           if (project != null)
           {
               LoggerBase.Log("Project collection already loaded and found: " + projectPath, isDebugMessage: true);
               return project;
           }

           return new Project(projectPath);
       }

    }
}
