using NHibernateRepo.Configuration;
using NHibernateRepo.Migrations;

namespace ExampleRepo.Migrations
{
    public class SessionRepoConfiguration : RepoMigrationConfigurationBase<SessionRepo>
    {
        public SessionRepoConfiguration()
        {
            Enabled = true;
            MigrationType = MigrationToUse.Manual;
            RootMigrationFolder = @"Migrations\SessionRepoMigrations";
        }
    }
}
