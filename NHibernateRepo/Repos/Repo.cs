using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using NHibernate;
using NHibernate.Linq;

namespace NHibernateRepo.Repos
{
    //Base repo is used so that all instances of the generic repo can be found through reflection without knowing the TEntity and TOverride.
    public abstract class BaseRepo
    {
        public string ConnectionStringOrName { get; protected set; }

        internal ISessionFactory SessionFactory;

        private ISession _session;
        internal ISession Session
        {
            get
            {
                if (_session == null || !_session.IsOpen)
                {                   
                    _session = SessionFactory.OpenSession();
                }

                return _session;
            }
        }
    }

    public abstract class RepoCombined<TEntity> : RepoSplit<TEntity, TEntity>, IRepoCombined<TEntity>
        where TEntity : class
    {
        protected RepoCombined(string connectionStringOrName) : base(connectionStringOrName)
        {
        }
    }

    public abstract class RepoSplit<TEntity, TOverride> 
        : BaseRepo, IRepoSplit<TEntity, TOverride>        
        where TEntity : class
        where TOverride : class
    {

        protected RepoSplit(string connectionStringOrName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringOrName))
            {
                throw new ArgumentNullException("connectionStringOrName", "Connection string or name must be provided to Repository Base");
            }

            ConnectionStringOrName = connectionStringOrName;
            var repoSetup = CreateRepoSetup(ConnectionStringOrName);
//            SessionFactory = repoSetup.SessionFactory;
        }

        public RepoTransaction<TEntity, TOverride> BeginTransaction()
        {
            return new RepoTransaction<TEntity, TOverride>(ConnectionStringOrName);
        }

        #region Entity methods


        public T Get<T, TId>(TId id)
        {
            return Session.Load<T>(id);
        }


        public void Update<T>(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "Cannot update null entity");

            Session.Update(entity);
            Session.Flush();
        }

        public void Create<T>(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "Cannot create null entity");

            Session.Save(entity);
            Session.Flush();
        }


        #endregion


        #region Project Queries


        public TP[] List<TE, TP>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Where(exp).Project().To<TP>().AsQueryable().ToArray();
        }

        public TP Single<TE, TP>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Where(exp).ProjectTo<TP>().Single();
        }

        public TP SingleOrDefault<TE, TP>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Where(exp).ProjectTo<TP>().SingleOrDefault();
        }

        public TP First<TE, TP>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Where(exp).ProjectTo<TP>().First();
        }

        public TP FirstOrDefault<TE, TP>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Where(exp).ProjectTo<TP>().FirstOrDefault();
        }

        public bool Any<TE>(Expression<Func<TE, bool>> exp)
        {
            return Session.Query<TE>().Any(exp);
        }


        #endregion



        #region Migration Methods

        internal RepoSetup<TEntity, TOverride> CreateRepoSetup(string conString)
        {
            return new RepoSetup<TEntity, TOverride>(conString);
        }


        #endregion
    }
}
