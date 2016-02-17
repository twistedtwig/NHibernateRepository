using NHibernateRepo.AutoMapper;
using NHibernateRepo.Migrations;

namespace NHibernateMigrationRepo
{
    internal class AutoMapperSettings : IAutoMapperSettings
    {
        internal static void MapMigrationLogs()
        {
            AutoMapper.Mapper.CreateMap<MigrationLogEntity, MigrationInfo>()
                .ForMember(l => l.Name, c => c.MapFrom(x => x.MigrationName));
        }
    }
}
