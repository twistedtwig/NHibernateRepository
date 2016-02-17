using System;
using System.Linq;
using NHibernateRepo.Migrations;
using NHibernateRepo.Repos;

namespace NHibernateMigrationRepo
{
    public class MigrationRepo : RepoCombined<MigrationLogEntity>
    {
        public MigrationRepo(string connectionStringOrName) : base(connectionStringOrName)
        {
            AutoMapperSettings.MapMigrationLogs();
        }

        public String[] GetMigrationsThatHaveRun(string repoName)
        {
            if (string.IsNullOrWhiteSpace(repoName))
            {
                throw new ArgumentOutOfRangeException("repoName", "RepoName must be provided and not null or empty");
            }

            return List<MigrationLogEntity, MigrationInfo>(l => l.RepoName == repoName).Select(l => l.Name).ToArray();
        }

        public void LogMigrationRan(string migrationName, string repoName)
        {
            if (string.IsNullOrWhiteSpace(migrationName))
            {
                throw new ArgumentNullException("migrationName", "Migration name can not be null or empty");
            }

            if (string.IsNullOrWhiteSpace(repoName))
            {
                throw new ArgumentNullException("repoName", "repo name can not be null or empty");
            }

            Create(new MigrationLogEntity
            {
                DateRun = DateTime.UtcNow,
                MigrationName = migrationName,
                RepoName = repoName
            });
        }
    }
}
