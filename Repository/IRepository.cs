//https://github.com/jamiewest/Repository
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<TEntity, TContext>
    {
        TContext Context { get; }

        IQueryable<TEntity> Entities { get; }

        int SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken);

        RepositoryResult Create(TEntity entity);

        Task<RepositoryResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Update(TEntity entity);

        Task<RepositoryResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Delete(TEntity entity);

        Task<RepositoryResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        TEntity FindByKey(params object[] keyValues);

        Task<TEntity> FindByKeyAsync(params object[] keyValues);

        Task<TEntity> FindByKeyAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> All();

        Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}