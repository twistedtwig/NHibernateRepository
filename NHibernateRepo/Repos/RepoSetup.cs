using System;
using System.Configuration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernateRepo.AutoMapper;
using NHibernateRepo.Configuration;
using NHibernateRepo.Migrations;

namespace NHibernateRepo.Repos
{
    internal class RepoSetup<TEntity, TOverride>
        where TEntity : class
        where TOverride : class
    {
        private ISessionFactory _sessionFactory;
        private readonly string _connectionString;

        public RepoSetup(string connectionStringOrName)
        {
            if (String.IsNullOrWhiteSpace(connectionStringOrName)) return;

            var conn = ConfigurationManager.ConnectionStrings[connectionStringOrName];
            _connectionString = conn != null ? conn.ConnectionString : connectionStringOrName;
        }

        internal ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory =
                        CreateConfiguration()
                        .BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        internal FluentConfiguration CreateConfiguration()
        {
            var config = Fluently.Configure()
                .Database(CreateDbConfig)
                .Mappings(m => m.AutoMappings.Add(CreateMappings()));
            
            return config;
        }

        private MsSqlConfiguration CreateDbConfig()
        {
            return MsSqlConfiguration
                .MsSql2012
                .ConnectionString(_connectionString);
        }

        private AutoPersistenceModel CreateMappings()
        {
            var mappingSetup = AutoMap
                .AssemblyOf<TEntity>()
                    .Where(x => 
                        !typeof(IAutoMapperSettings).IsAssignableFrom(x)  //ignore all classes that implement auto mapper settings.
                        && !x.IsSubclassOf(typeof(BaseRepo))  //ignore all classes that inherit from nhibernate base repo class
                        && !x.IsSubclassOf(typeof(RepoSplit<TEntity, TOverride>))  //ignore all classes that inherit from nhibernate repo class
                        && !x.IsSubclassOf(typeof(RepoCombined<TEntity>))  //ignore all classes that inherit from nhibernate repo class
                        && !typeof(IClassConvention).IsAssignableFrom(x) //ignore all NHibernate convention classes
                        && !x.IsSubclassOf(typeof(AbstractBaseMigration)) //ignore all migration files.
                        && !x.IsSubclassOf(typeof(RepoMigrationConfigurationBaseNoneGeneric)) //ignore all migration configuration files.
                        )
                    
                .Conventions.AddFromAssemblyOf<TOverride>()
                
                .UseOverridesFromAssemblyOf<TOverride>()
                .Conventions.Add(DefaultLazy.Never())
                .Conventions.Add(DefaultCascade.SaveUpdate());
            
            return mappingSetup;
        }

    }
}
