//https://github.com/jamiewest/Repository
//https://github.com/tichaczech/fluff/blob/48482dc38efd04040f6798c318563a6e8ba3d244/Data/src/Repositories/BaseRepository.cs
//https://github.com/davidegironi/dgdatamodel/blob/66c08f2eb129abd4d84c496502f9338f0c85ec4c/DGDataModel/IGenericDataRepository.cs
//https://github.com/radixwb/RxEntityFrameworkCore/blob/92f1e3e8341135174eb4d747fba5b4539b985576/Rx.EntityFrameworkCore/Repository/Repository.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<T> : Repository<T, DbContext> where T : class
    {
        public Repository(DbContext context) : base(context) { }
    }
    
    public class Repository<T, TContext> : IRepository<T, TContext> where T : class where TContext : DbContext
    {
        public Repository(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private bool _disposed;
        public TContext Context { get; }
        public virtual IQueryable<T> Entities => Context.Set<T>();

        public bool AutoSaveChanges { get; set; } = true;

        public int SaveChanges()
        {
            if (AutoSaveChanges)
            {
                int i = Context.SaveChanges();
                return i;
            }
            return 0;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual RepositoryResult Create(T entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Add(entity);
            SaveChanges();
            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> CreateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Add(entity);
            await SaveChangesAsync(cancellationToken);
            return RepositoryResult.Success;
        }

        public virtual RepositoryResult Update(T entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Attach(entity);
            Context.Update(entity);
            SaveChanges();

            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Attach(entity);
            // TODO: Update concurrency stamp if any on the entity.
            Context.Update(entity);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult.Failed(new RepositoryError {Code = "", Description = ""});
            }

            return RepositoryResult.Success;
        }

        public virtual RepositoryResult Delete(T entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Remove(entity);
            SaveChanges();

            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Remove(entity);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult.Failed(new RepositoryError {Code = "", Description = ""});
            }

            return RepositoryResult.Success;
        }

        public virtual T FindByKey(params object[] keyValues)
        {
            ThrowIfDisposed();
            return Context.Set<T>().Find(keyValues);
        }

        public virtual async Task<T> FindByKeyAsync(params object[] keyValues)
        {
            ThrowIfDisposed();
            return await Context.Set<T>().FindAsync(keyValues);
        }

        public virtual async Task<T> FindByKeyAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Context.Set<T>().FindAsync(keyValues, cancellationToken);
        }
 
        public virtual IEnumerable<T> All()
        {
            ThrowIfDisposed();
            return Entities.AsNoTracking().ToList();
        }

        public virtual IEnumerable<T> By(Expression<Func<T, bool>> predicate)
        {
            ThrowIfDisposed();
            return Context.Set<T>().AsNoTracking().Where<T>(predicate).ToList<T>();
        }

        public virtual IEnumerable<T> By(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            ThrowIfDisposed();
            var query = Context.Set<T>().AsNoTracking<T>().AsQueryable<T>();
            foreach (string navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);
            return query.Where<T>(predicate).ToList<T>();
        }

        public virtual int ExecuteSqlCommand(string rawSqlString, params object[] paramaters)
        {
            ThrowIfDisposed();
            int i = Context.Database.ExecuteSqlCommand(rawSqlString, paramaters);
            return i;
        }

        public virtual async Task<IEnumerable<T>> AllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            return await Entities.AsNoTracking().ToListAsync(cancellationToken);
        }

        internal IQueryable<T> AggregateProperties(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(Entities.AsNoTracking(), (current, includeProperty) => current.Include(includeProperty));
        }

        internal void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose() => _disposed = true;
    }
}