using System;
using System.IO;
using System.Linq;
using DatabaseManagement;
using DatabaseManagement.Logging;
using DatabaseManagement.Models;

namespace TestConsoleWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        private void Run()
        {
            CopyDllsForDomainLoadingDebug(@"D:\Development\NHibernateRepoProject\Test\ExampleRepo\bin\debug");

            LoggerBase.IsDebugging = true;
            var setup = new SchemaSetup();
                                                                    
//            setup.EnableMigrations(new EnableMigrationsCriteria
//            {
//                ProjectPath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\examplerepo.csproj",
//                RepoName = "examplerepo",
//                ConfigFilePath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\App.config",                
//            });



//            var criteria = new CreationCriteria
//            {
//                ProjectFileLocation = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\ExampleRepo.csproj",
//                FileName = "initial_Setup-Models",
//                RepoName = "examplerepo",
//                ConfigFilePath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\App.config"
//            };
//            setup.CreateScript(criteria);


//            var criteria = new CreationCriteria
//            {
//                ProjectFileLocation = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\ExampleRepo.csproj",
//                FileName = "Age_added",
//                RepoName = "ExampleRepo",
//                ConfigFilePath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\App.config"
//            };
//            setup.CreateScript(criteria);



                var crit = new ApplyMigrationCriteria
                {
                    ProjectPath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\ExampleRepo.csproj",
                    RepoName = "ExampleRepo",
                    ConfigFilePath = @"D:\Development\NHibernateRepoProject\Test\ExampleRepo\App.config"
                };
            
                setup.ApplyMigrations(crit);



        }

        /// <summary>
        /// This is required for debugging as NHibernate needs to load the DLLs and seems to only look in the current folder.
        /// When run live it will be run via powershell which will put the EXE in the destination folder.
        /// Debugging will have to be run through here, NHMigrate will not work correctly.
        /// </summary>
        /// <param name="projectFolder"></param>
        private void CopyDllsForDomainLoadingDebug(string projectFolder)
        {            
            var projectDirectory = new DirectoryInfo(projectFolder);

            var executingBinFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

            var currentFilesInDestination = new DirectoryInfo(executingBinFolder).GetFiles("*.dll").Select(f => f.Name).ToList();
            var dlls = projectDirectory.GetFiles("*.dll");

            foreach (var fileInfo in dlls)
            {
                var destFileName = Path.Combine(executingBinFolder, fileInfo.Name);

                if (currentFilesInDestination.Contains(fileInfo.Name))
                {
                    try
                    {
                        File.Delete(destFileName);
                    }
                    catch (Exception)
                    {
                        //try and delete the old files, ignore errors
                    }
                };

                File.Copy(fileInfo.FullName, destFileName);
            }
        }

    }


}
