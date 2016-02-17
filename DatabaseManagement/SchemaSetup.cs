using DatabaseManagement.Configuration;
using DatabaseManagement.EnvDte;
using DatabaseManagement.Logging;
using DatabaseManagement.Migrations;
using DatabaseManagement.Models;
using DatabaseManagement.ProjectHelpers;
using DatabaseManagement.SqlDb;
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
                LoggerBase.Log("Migrations are not enabled");
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
            ProjectEvalutionHelper.FinishedWithProject(criteria.ProjectFilePath);
        }


        /// <summary>
        /// Enables the ability for that project to be able to use NHibernate Repo migrations, automatic or manual.
        /// </summary>
        public void EnableMigrations(EnableMigrationsCriteria criteria)
        {
            MessageFilter.Register();

            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectPath, criteria.RepoName);
            //if it is null something is wrong so drop out.
            if (repoInfo == null) return;

            AssertRepoHasEmptyConstructor(repoInfo.RepoType);
            EnsureDbAndMigrationTableExists(criteria.ProjectPath, repoInfo.RepoType, criteria.ConfigFilePath);

            var repoBase = TypeHandler.CreateRepoBase(repoInfo.Assembly.Location, repoInfo.RepoType);

            //create migration log table, if it doesn't exist.
            var updater = CreateSchemaUpdater(criteria.ProjectPath, typeof(MigrationRepo), criteria.ConfigFilePath, repoBase.ConnectionStringOrName);
            updater.Execute(true, true);

            bool multipleFound = false;
            var configType = TypeHandler.FindSingleConfiguration(criteria.ProjectPath, repoInfo.RepoType, out multipleFound);
            
            //this should not happen. should only ever have one config per repo type.
            if(multipleFound) return;

            if (configType == null)
            {
                LoggerBase.Log("Adding migration configuration");
                var filePath = new ConfigurationFileHandler().CreateConfigurationFile(criteria.ProjectPath, repoInfo.RepoType.Name, "Migrations", MigrationToUse.Manual);
                new ProjectDteHelper().AddFile(criteria.ProjectPath, "Migrations", filePath, showFile: true);                
            }
            else
            {
                LoggerBase.Log("System is already configured for migrations, see class: " + configType.Name);
            }
            
            //clean up
            ProjectEvalutionHelper.FinishedWithProject(criteria.ProjectPath);
            MessageFilter.Revoke();
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
                LoggerBase.Log("Migrations are not enabled, can not apply any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Automatic)
            {
                LoggerBase.Log("automatic Migrations are not enabled, can not apply any migrations.");
                return;
            }

            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectFilePath, criteria.RepoName);
            //if null we need to drop out.
            if (repoInfo == null) return;

            AssertRepoHasEmptyConstructor(repoInfo.RepoType);
            EnsureDbAndMigrationTableExists(criteria.ProjectFilePath, repoInfo.RepoType, criteria.ConfigFilePath);

            var updater = CreateSchemaUpdater(repoInfo.Assembly.Location, repoInfo.RepoType, criteria.ConfigFilePath);    
            updater.Execute(true, true);

            //clean up
            ProjectEvalutionHelper.FinishedWithProject(criteria.ProjectFilePath);
        }

        private static void AssertRepoHasEmptyConstructor(Type repoType)
        {
            if (!TypeHandler.DoesTypeHaveEmptyConstructor(repoType))
            {
                throw new ApplicationException(string.Format("Repo type '{0}' must have an empty constructor", repoType));
            }
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
                LoggerBase.Log("Migrations are not enabled, can not create any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Manual)
            {
                LoggerBase.Log("Manual Migrations are not enabled, can not create manual migrations.");
                return;
            }
            
            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectFileLocation, criteria.RepoName);
            //if null we need to drop out.
            if (repoInfo == null)
            {
                LoggerBase.Log("Unable to find repo");
                return;
            }
            
            AssertRepoHasEmptyConstructor(repoInfo.RepoType);
            EnsureDbAndMigrationTableExists(criteria.ProjectFileLocation, repoInfo.RepoType, criteria.ConfigFilePath);
            
            //ensure that we have the case correct.
            criteria.RepoName = repoInfo.RepoType.Name;

            var configuration = TypeHandler.FindConfiguration(criteria.ProjectFileLocation, repoInfo.RepoType);
            //if null something bad happend, drop out
            if (configuration == null)
            {
                LoggerBase.Log("unable to find configuration file");
                return;
            }

            var updater = CreateSchemaUpdater(repoInfo.Assembly.Location, repoInfo.RepoType, criteria.ConfigFilePath);    
       
            var fileMigrationHandler = new MigrationFileHandler(updater);
            var projectFileHandler = new ProjectDteHelper();

            //set the mgiration folder form config:
            criteria.MigrationPath = configuration.RootMigrationFolder;

            var filePath = fileMigrationHandler.CreateFile(criteria);           
            projectFileHandler.AddFile(criteria.ProjectFileLocation, configuration.RootMigrationFolder, filePath, showFile: true);
            LoggerBase.Log("Created migration file.");

            //clean up
            ProjectEvalutionHelper.FinishedWithProject(criteria.ProjectFileLocation);
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
                LoggerBase.Log("Migrations are not enabled, can not apply any migrations.");
                return;
            }

            if (configurationStatus.MigrationType != MigrationToUse.Manual)
            {
                LoggerBase.Log("Manual Migrations are not enabled, can not apply any migrations.");
                return;
            }

            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectPath, criteria.RepoName);
            //if null we need to drop out.
            if (repoInfo == null) return;

            AssertRepoHasEmptyConstructor(repoInfo.RepoType);
            EnsureDbAndMigrationTableExists(criteria.ProjectPath, repoInfo.RepoType, criteria.ConfigFilePath);
            
            var runner = new MigrationRunner();
            runner.ApplyMigrations(criteria);

            //clean up
            ProjectEvalutionHelper.FinishedWithProject(criteria.ProjectPath);
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
        private SchemaUpdate CreateSchemaUpdater(string projectdllPath, Type repoType, string configFilePath, params object[] args)
        {
            var repoBase = TypeHandler.CreateRepoBase(projectdllPath, repoType, args);
            MethodInfo createRepoSetupMethod = repoType.GetMethod("CreateRepoSetup", BindingFlags.Instance | BindingFlags.NonPublic);
            var connectionString = ConnectionStringHandler.FindConnectionString(repoBase, configFilePath);

           var repoSetup = createRepoSetupMethod.Invoke(repoBase, new object[] { connectionString }); 

            var createConfigMethod = repoSetup.GetType().GetMethod("CreateConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);
            var config = createConfigMethod.Invoke(repoSetup, null) as FluentConfiguration;
            
            var updater = new SchemaUpdate(config.BuildConfiguration());
            return updater;
        }

        /// <summary>
        /// Ensures that the migration database exists.
        /// </summary>
        /// <param name="projectdllPath"></param>
        /// <param name="repoType"></param>
        /// <param name="configFilePath"></param>
        /// <param name="args"></param>
        private void CreateRepoDatabase(string projectdllPath, Type repoType, string configFilePath, params object[] args)
        {
            var repoBase = TypeHandler.CreateRepoBase(projectdllPath, repoType, args);
            var connectionString = ConnectionStringHandler.FindConnectionString(repoBase, configFilePath);

            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            string databaseName = builder.InitialCatalog;
            
            var migrationDbExists = DatabaseChecking.CheckDatabaseExists(connectionString, databaseName);
            if (!migrationDbExists)
            {
                DatabaseCreation.CreateDatabase(connectionString, databaseName);
            }
        }

        private void EnsureDbAndMigrationTableExists(string projectdllPath, Type repoType, string configFilePath)
        {
            var repoBase = TypeHandler.CreateRepoBase(projectdllPath, repoType);  //this might blow up.. might need "args"
            var connectionString = ConnectionStringHandler.FindConnectionString(repoBase, configFilePath);

            CreateRepoDatabase(projectdllPath, repoType, configFilePath);
            //create migration log table, if it doesn't exist.
            var updateMigration = CreateSchemaUpdater(projectdllPath, typeof(MigrationRepo), configFilePath, connectionString);
            updateMigration.Execute(true, true);
        }
        
    }
}
