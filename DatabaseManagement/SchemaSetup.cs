using DatabaseManagement.Configuration;
using DatabaseManagement.Migrations;
using DatabaseManagement.Models;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateMigrationRepo;
using NHibernateRepo.Migrations;
using System;
using System.Reflection;

namespace DatabaseManagement
{
    public class SchemaSetup
    {

        /// <summary>
        /// Applies the database update, be that automatic or manual according to the configuration class.
        /// </summary>
        /// <param name="criteria"></param>
        public void UpdateDatabase(UpdateDatabaseCriteria criteria)
        {
            var status = GetMigrationConfigurationStatus(criteria.ProjectFilePath, criteria.RepoName);
            if (!status.Enabled)
            {
                Console.WriteLine("Migrations are not enabled");
                return;
            }

            switch (status.MigrationType)
            {
                case MigrationToUse.Automatic:
                    UpdateDatabaseAutomatically(new AutomaticUpdateCriteria
                    {
                        ProjectFilePath = criteria.ProjectFilePath,
                        RepoName = criteria.RepoName
                    });
                    break;
                case MigrationToUse.Manual:
                    ApplyMigrations(new ApplyMigrationCriteria
                    {
                        ProjectPath = criteria.ProjectFilePath,
                        RepoName = criteria.RepoName
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            //clean up
            new ProjectFileHandler().FinishedWithProject(criteria.ProjectFilePath);
        }


        /// <summary>
        /// Enables the ability for that project to be able to use NHibernate Repo migrations, automatic or manual.
        /// </summary>
        public void EnableMigrations(EnableMigrationsCriteria criteria)
        {            
            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectPath, criteria.RepoName);
            //if it is null something is wrong so drop out.
            if (repoInfo == null) return;

            bool multipleFound = false;
            var configType = TypeHandler.FindSingleConfiguration(criteria.ProjectPath, repoInfo.RepoType, out multipleFound);
            
            //this should not happen. should only ever have one config per repo type.
            if(multipleFound) return;

            if (configType == null)
            {
                Console.WriteLine("Adding migration configuration");
                var filePath = new ConfigurationFileHandler().CreateConfigurationFile(criteria.ProjectPath, repoInfo.RepoType.Name, "Migrations", MigrationToUse.Manual);
                new ProjectFileHandler().AddFile(criteria.ProjectPath, "Migrations", filePath);                
            }
            else
            {
                Console.WriteLine("System is already configured for migrations, see class: " + configType.Name);
            }

            var repoBase = TypeHandler.CreateRepoBase(repoInfo.Assembly.Location, repoInfo.RepoType);
            
            //create migration log table, if it doesn't exist.
            var updater = CreateSchemaUpdater(criteria.ProjectPath, typeof(MigrationRepo), repoBase.ConnectionStringOrName);
            updater.Execute(false, true);

            //clean up
            new ProjectFileHandler().FinishedWithProject(criteria.ProjectPath);
        }







        /// <summary>
        /// Automatically update the database schema to bring it up to date.
        /// </summary>
        /// <param name="criteria">creation criteria</param>
        public void UpdateDatabaseAutomatically(AutomaticUpdateCriteria criteria)
        {
            var configurationStatus = GetMigrationConfigurationStatus(criteria.ProjectFilePath, criteria.RepoName);
            if (!configurationStatus.Enabled)
            {
                Console.WriteLine("Migrations are not enabled, can not apply any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Automatic)
            {
                Console.WriteLine("automatic Migrations are not enabled, can not apply any migrations.");
                return;
            }

            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectFilePath, criteria.RepoName);
            //if null we need to drop out.
            if (repoInfo == null) return;
          
            var updater = CreateSchemaUpdater(repoInfo.Assembly.Location, repoInfo.RepoType);    
            updater.Execute(false, true);

            //clean up
            new ProjectFileHandler().FinishedWithProject(criteria.ProjectFilePath);
        }

        /// <summary>
        /// Creates a CS file that inherits from BaseMigration that will execute the SQL schema changes.
        /// </summary>
        /// <param name="criteria">description of what to do and where to put the file</param>
        public void CreateScript(CreationCriteria criteria) 
        {
            var configurationStatus = GetMigrationConfigurationStatus(criteria.ProjectFileLocation, criteria.RepoName);
            if (!configurationStatus.Enabled)
            {
                Console.WriteLine("Migrations are not enabled, can not create any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Manual)
            {
                Console.WriteLine("Manual Migrations are not enabled, can not create manual migrations.");
                return;
            }
            
            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectFileLocation, criteria.RepoName);
            //if null we need to drop out.
            if(repoInfo == null) return;

            var configuration = TypeHandler.FindConfiguration(criteria.ProjectFileLocation, repoInfo.RepoType);
            //if null something bad happend, drop out
            if(configuration == null) return;

            var updater = CreateSchemaUpdater(repoInfo.Assembly.Location, repoInfo.RepoType);    
       
            var fileMigrationHandler = new MigrationFileHandler(updater);
            var projectFileHandler = new ProjectFileHandler();

            //set the mgiration folder form config:
            criteria.MigrationPath = configuration.RootMigrationFolder;

            var filePath = fileMigrationHandler.CreateFile(criteria);           
            projectFileHandler.AddFile(criteria.ProjectFileLocation, configuration.RootMigrationFolder, filePath);


            //clean up
            projectFileHandler.FinishedWithProject(criteria.ProjectFileLocation);
        }


        /// <summary>
        /// Applies all migrations for the given repository.
        /// </summary>
        /// <param name="criteria"></param>
        public void ApplyMigrations(ApplyMigrationCriteria criteria)
        {
            var configurationStatus = GetMigrationConfigurationStatus(criteria.ProjectPath, criteria.RepoName);
            if (!configurationStatus.Enabled)
            {
                Console.WriteLine("Migrations are not enabled, can not apply any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Manual)
            {
                Console.WriteLine("Manual Migrations are not enabled, can not apply any migrations.");
                return;
            }
            
            var runner = new MigrationRunner();
            runner.ApplyMigrations(criteria);

            //clean up
            new ProjectFileHandler().FinishedWithProject(criteria.ProjectPath);
        }


        private ConfigurationStatus GetMigrationConfigurationStatus(string projectPath, string optionalRepo)
        {
            var status = new ConfigurationStatus();

            var configObject = TypeHandler.FindConfiguration(projectPath, optionalRepo);
            if (configObject == null)
            {
                status.Enabled = false;
                return status;
            }

            status.Enabled = configObject.Enabled;
            status.MigrationType = configObject.MigrationType;

            return status;
        }

        /// <summary>
        /// Creates an instance of the NHibernate SchemaUpdate class using the repository found.
        /// </summary>
        /// <param name="projectdllPath"></param>
        /// <param name="repoType"></param>
        /// <param name="args">optional constructor arguments</param>
        /// <returns></returns>
        private SchemaUpdate CreateSchemaUpdater(string projectdllPath, Type repoType, params object[] args)
        {
            var repoBase = TypeHandler.CreateRepoBase(projectdllPath, repoType, args);
            MethodInfo createRepoSetupMethod = repoType.GetMethod("CreateRepoSetup", BindingFlags.Instance | BindingFlags.NonPublic);
            var repoSetup = createRepoSetupMethod.Invoke(repoBase, null);

            var createConfigMethod = repoSetup.GetType().GetMethod("CreateConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);
            var config = createConfigMethod.Invoke(repoSetup, null) as FluentConfiguration;

            var updater = new SchemaUpdate(config.BuildConfiguration());
            return updater;
        }
        
    }
}
