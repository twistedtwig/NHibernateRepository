using System;
using NHibernate;

namespace NHibernateRepo.Repos
{
    public class RepoTransaction<TEntity, TOverride> : RepoSplit<TEntity, TOverride>, IRepoTransaction<TEntity, TOverride> , IDisposable
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