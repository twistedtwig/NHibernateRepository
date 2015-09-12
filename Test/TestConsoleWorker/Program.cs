using System;
using System.Collections.Generic;
using DatabaseManagement;
using DatabaseManagement.Models;

namespace TestConsoleWorker
{
    class Program
    {
        private static List<string> _libs = new List<string>(); 
        private static string testpath = "";
        static void Main(string[] args)
        {            
            var p = new Program();
            p.Run();
        }

        private void Run()
        {
            var setup = new SchemaSetup();
                                                                    
//            setup.EnableMigrations(new EnableMigrationsCriteria
//            {
//                ProjectPath = @"E:\Work\nhibernateRepo\NHibernateRepository\Test\ExampleRepo\examplerepo.csproj",
//                RepoName = "examplerepo"
//            });






            var criteria = new CreationCriteria
            {
                ProjectFileLocation = @"E:\Work\nhibernateRepo\NHibernateRepository\Test\ExampleRepo\ExampleRepo.csproj",
                FileName = "Age_added",
                RepoName = "ExampleRepo"
            };
            setup.CreateScript(criteria);



//                var crit = new ApplyMigrationCriteria
//                {
//                    ProjectPath = @"E:\Work\nhibernateRepo\NHibernateRepository\Test\ExampleRepo\ExampleRepo.csproj",
//                    RepoName = "ExampleRepo"
//                };
//            
//                setup.ApplyMigrations(crit);
        }
    }


}
