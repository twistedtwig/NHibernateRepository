using System;
using System.Linq.Expressions;

namespace NHibernateRepo
{
    public interface IRepo<TEntity, TOverride>
        where TEntity : class
        where TOverride : class
    {
        RepoTransaction<TEntity, TOverride> BeginTransaction();

        T Get<T, TId>(TId id);

        void Update<T>(T entity);
        void Create<T>(T entity);

        TP[] List<TE, TP>(Expression<Func<TE, bool>> exp);
        TP Single<TE, TP>(Expression<Func<TE, bool>> exp);
        TP SingleOrDefault<TE, TP>(Expression<Func<TE, bool>> exp);
        TP First<TE, TP>(Expression<Func<TE, bool>> exp);
        TP FirstOrDefault<TE, TP>(Expression<Func<TE, bool>> exp);
    }

    public interface IRepoTransaction<TEntity, TOverride> : IDisposable
        where TEntity : class
        where TOverride : class
    {

        void Commit();
        void Rollback();
    }
}
