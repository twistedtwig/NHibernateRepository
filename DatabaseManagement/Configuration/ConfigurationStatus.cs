using NHibernateRepo.Migrations;

namespace DatabaseManagement.Configuration
{
    internal class ConfigurationStatus
    {
        public bool Enabled { get; set; }
        public MigrationToUse MigrationType { get; set; }
    }
}
