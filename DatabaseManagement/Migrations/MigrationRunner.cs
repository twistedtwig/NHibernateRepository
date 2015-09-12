using DatabaseManagement.Models;
using NHibernateMigrationRepo;
using NHibernateRepo.Migrations;
using System;
using System.Linq;
using System.Reflection;

namespace DatabaseManagement.Migrations
{
    public class MigrationRunner
    {
        
        public void ApplyMigrations(ApplyMigrationCriteria criteria)
        {
            var repoInfo = TypeHandler.FindSingleRepo(criteria.ProjectPath, criteria.RepoName);
            if (repoInfo == null)
            {
                return;
            }

            var repoBase = TypeHandler.CreateRepoBase(repoInfo.Assembly.Location, repoInfo.RepoType);

            var migrationTypes = TypeHandler.FindAllMigrations(criteria.ProjectPath, repoInfo.RepoType);

            if (migrationTypes == null || !migrationTypes.Any())
            {
                Console.WriteLine("No migrations to run.");
                return;
            }

            var migrationRepo = new MigrationRepo(repoBase.ConnectionStringOrName);
            var doneMigrations = migrationRepo.GetMigrationsThatHaveRun(repoInfo.RepoType.Name);
            //only run on migrations that have not been run yet.
            //order by name, ie. datestamp.
            foreach (var migrationType in migrationTypes.Where(m => !doneMigrations.Contains(m.Name)).OrderBy(m => m.Name))
            {
                var migration = Activator.CreateInstance(migrationType) as AbstractBaseMigration;
                if (migration == null)
                {
                    Console.WriteLine("Error creating migration: " + migrationType.Name);
                    throw new ApplicationException("Error creating migration: " + migrationType.Name);
                }

                //use reflection to set the internal base repo
                PropertyInfo baseRepoPropertyInfo = migrationType.GetProperty("BaseRepo", BindingFlags.Instance | BindingFlags.NonPublic);
                baseRepoPropertyInfo.SetValue(migration, repoBase);

                Console.WriteLine("Executing Migration: {0}", migrationType.Name);
                migration.Execute();
                migrationRepo.LogMigrationRan(migrationType.Name, repoInfo.RepoType.Name);
            }
        }
        
    }
}
