using NHibernateRepo.Migrations;
using NHibernateRepo.Repos;

namespace NHibernateRepo.Configuration
{
    public abstract class RepoMigrationConfigurationBaseNoneGeneric
    {
        public bool Enabled { get; protected set; }
        public MigrationToUse MigrationType { get; protected set; }
        public string RootMigrationFolder { get; protected set; }
    }

    public abstract class RepoMigrationConfigurationBase<T> : RepoMigrationConfigurationBaseNoneGeneric 
        where T : BaseRepo
    {
        
    }

    
}
