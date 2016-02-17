using System;
using System.Linq;
using System.Linq.Expressions;

namespace NHibernateRepo.Repos
{
    public interface IRepoCombined<TEntity> : IRepoSplit<TEntity, TEntity> 
        where TEntity : class
    {
        
    }


    public interface IRepoSplit<TEntity, TOverride>
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
        bool Any<TE>(Expression<Func<TE, bool>> exp);
    }

    public interface IRepoTransaction<TEntity, TOverride> : IDisposable
        where TEntity : class
        where TOverride : class
    {
        IQueryable<T> Entities<T>();
        void Commit();
        void Rollback();
    }
}
