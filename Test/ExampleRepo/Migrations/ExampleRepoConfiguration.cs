using NHibernateRepo.Configuration;
using NHibernateRepo.Migrations;

namespace ExampleRepo.Migrations
{
    public class ExampleRepoConfiguration : RepoMigrationConfigurationBase<ExampleRepo>
    {
        public ExampleRepoConfiguration()
        {
            Enabled = true;
            MigrationType = MigrationToUse.Manual;
            RootMigrationFolder = @"Migrations\ExampleRepoMigrations";
        }
    }
}
