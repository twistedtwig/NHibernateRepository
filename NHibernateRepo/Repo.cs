using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using NHibernate;
using NHibernate.Linq;

namespace NHibernateRepo
{
    public abstract class Repo<TEntity, TOverride>
        : IRepo<TEntity, TOverride> 
        where TEntity : class
        where TOverride : class
    {
        private readonly string _connectionStringOrName;
        private readonly ISessionFactory _sessionFactory;

        private ISession _session;

        protected ISession Session
        {
            get
            {
                if (_session == null || !_session.IsOpen)
                {
                    _session = _sessionFactory.OpenSession();
                }

                return _session;
            }
        }


        protected Repo(string connectionStringOrName)
        {
            _connectionStringOrName = connectionStringOrName;
            var repoSetup = new RepoSetup<TEntity, TOverride>(_connectionStringOrName);
            _sessionFactory = repoSetup.SessionFactory;
        }

        public RepoTransaction<TEntity, TOverride> BeginTransaction()
        {
            return new RepoTransaction<TEntity, TOverride>(_connectionStringOrName);
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
    }

    public class RepoTransaction<TEntity, TOverride> : Repo<TEntity, TOverride>, IRepoTransaction<TEntity, TOverride> , IDisposable
        where TEntity : class
        where TOverride : class
    {
        private ITransaction _transaction;


        internal RepoTransaction(string connectionStringOrName)
            : base(connectionStringOrName)
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                if (_transaction.IsActive)
                {
                    _transaction.Commit();
                }
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                if (_transaction.IsActive)
                {
                    _transaction.Rollback();
                }
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                if (_transaction.IsActive)
                {
                    _transaction.Rollback();
                }

                _transaction = null;
            }
        }
    }
}
