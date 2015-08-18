using System;
using System.Configuration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;


namespace NHibernateRepo
{
    public class RepoSetup<TEntity, TOverride>
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

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(CreateDbConfig)
                        .Mappings(m => m.AutoMappings.Add(CreateMappings()))
                        .BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        private MsSqlConfiguration CreateDbConfig()
        {
            return MsSqlConfiguration
                .MsSql2012
                .ConnectionString(_connectionString);
        }

        private AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .AssemblyOf<TEntity>()
                .UseOverridesFromAssemblyOf<TOverride>()
                .Conventions.Add(DefaultLazy.Never())
                .Conventions.Add(DefaultCascade.SaveUpdate());
        }
    }
}
