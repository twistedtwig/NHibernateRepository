using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace NHibernateMigrationRepo
{
    public class MigrationLogEntityOverride : IAutoMappingOverride<MigrationLogEntity>
    {
        public void Override(AutoMapping<MigrationLogEntity> mapping)
        {
            mapping.Table("_MigrationLogs");
        }
    }
}
