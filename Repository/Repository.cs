//https://github.com/jamiewest/Repository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Creates a new instance of a persistence store for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the class representing an entity</typeparam>
    public class Repository<TEntity> : Repository<TEntity, DbContext>
        where TEntity : class
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new instance of a persistence store for entities.
        /// </summary>
        public Repository(DbContext context) : base(context)
        {
        }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the class representing an entity.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a new instance of a persistence store for entities.
        /// </summary>
        public Repository(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private bool _disposed;

        /// <summary>
        /// Gets the database context for this store.
        /// </summary>
        public TContext Context { get; }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Entities => Context.Set<TEntity>();

        /// <summary>
        /// True if changes should be automatically persisted, otherwise false.
        /// </summary>
        public bool AutoSaveChanges { get; set; } = true;

        /// <inheritdoc />
        public int SaveChanges()
        {
            if (AutoSaveChanges)
            {
                int i = Context.SaveChanges();
                return i;
            }
            return 0;
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public virtual RepositoryResult Create(TEntity entity)
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

        /// <inheritdoc />
        public virtual async Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
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

        /// <inheritdoc />
        public virtual RepositoryResult Update(TEntity entity)
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

        /// <inheritdoc />
        public virtual async Task<RepositoryResult> UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
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

        /// <inheritdoc />
        public virtual RepositoryResult Delete(TEntity entity)
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

        /// <inheritdoc />
        public virtual async Task<RepositoryResult> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
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

        /// <inheritdoc />
        public virtual TEntity FindByKey(params object[] keyValues)
        {
            ThrowIfDisposed();
            return Context.Set<TEntity>().Find(keyValues);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByKeyAsync(params object[] keyValues)
        {
            ThrowIfDisposed();
            return await Context.Set<TEntity>().FindAsync(keyValues);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByKeyAsync(
            object[] keyValues,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Context.Set<TEntity>().FindAsync(keyValues, cancellationToken);
        }
 
        /// <inheritdoc />
        public virtual IEnumerable<TEntity> All()
        {
            ThrowIfDisposed();
            return Entities.AsNoTracking().ToList();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> AllAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            return await Entities.AsNoTracking().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Includes related navigation properties.
        /// </summary>
        internal IQueryable<TEntity> AggregateProperties(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(
                Entities.AsNoTracking(),
                (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        /// Throws if this class has been disposed
        /// </summary>
        internal void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the stores
        /// </summary>
        public void Dispose() => _disposed = true;
    }
}