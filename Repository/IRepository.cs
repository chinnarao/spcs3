//https://github.com/jamiewest/Repository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T, TContext>
    {
        TContext Context { get; }

        IQueryable<T> Entities { get; }

        int SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken);

        RepositoryResult Create(T entity);

        Task<RepositoryResult> CreateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Update(T entity);

        Task<RepositoryResult> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Delete(T entity);

        Task<RepositoryResult> DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        T FindByKey(params object[] keyValues);

        Task<T> FindByKeyAsync(params object[] keyValues);

        Task<T> FindByKeyAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<T> All();

        Task<IEnumerable<T>> AllAsync(CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<T> By(Expression<Func<T, bool>> predicate);

        IEnumerable<T> By(Expression<Func<T, bool>> predicate, params string[] navigationProperties);

        int ExecuteSqlCommand(string rawSqlString, params object[] paramaters);
    }
}