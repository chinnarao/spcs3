using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Extensions
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return repository.Entities.AsNoTracking()
                .Where(filter)
                .ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return repository.AggregateProperties(includeProperties)
                .Where(filter)
                .ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return orderBy(repository.Entities
                    .AsNoTracking()
                    .Where(filter))
                .ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.Entities
                .AsNoTracking()
                .Where(filter));

            if (maxRecords != 0)
            {
                return query.ToList().Take(maxRecords);
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return orderBy(repository.AggregateProperties(includeProperties)
                    .Where(filter))
                .ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.AggregateProperties(includeProperties)
                .Where(filter));

            if (maxRecords != 0)
            {
                return query.ToList()
                    .Take(maxRecords);
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.Entities
                .AsNoTracking()
                .Where(filter);

            if (maxRecords != 0)
            {
                return query.ToList().Take(maxRecords);
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/>. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> that represents the result of the query, a list of entities.
        /// </returns>
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.AggregateProperties(includeProperties)
                .Where(filter);

            if (maxRecords != 0)
            {
                query.Take(maxRecords);
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var results = await repository.Entities
                .AsNoTracking()
                .Where(filter)
                .ToListAsync(cancellationToken);

            return results;
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return await repository.AggregateProperties(includeProperties)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return await orderBy(repository.Entities.AsNoTracking().Where(filter))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="maxRecords">The maximum number of records to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.Entities.AsNoTracking().Where(filter));

            if (maxRecords != 0)
            {
                var results = await query.ToListAsync(cancellationToken);
                return results.Take(maxRecords);
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return await orderBy(repository.AggregateProperties(includeProperties)
                    .Where(filter))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="orderBy">The order criteria for which to return the entities.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            if (maxRecords != 0)
            {
                var results = await orderBy(repository.AggregateProperties(includeProperties)
                        .Where(filter))
                    .ToListAsync(cancellationToken);

                return results.Take(maxRecords);
            }

            return await orderBy(repository.AggregateProperties(includeProperties)
                    .Where(filter))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.Entities.AsNoTracking().Where(filter);
            if (maxRecords == 0)
            {
                return await query.ToListAsync(cancellationToken);
            }

            var results = await query.ToListAsync(cancellationToken);
            return results.Take(maxRecords);

        }

        /// <summary>
        /// Gets a list of entities that match the specified <paramref name="filter"/> as an asynchronous operation. 
        /// </summary>
        /// <param name="filter">The filter criteria for which entities to retrieve.</param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <param name="includeProperties">The related entities to retrieve.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that represents the result of the asynchronous query, a list of entities.
        /// </returns>
        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this Repository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            var query = repository.Entities.AsNoTracking().Where(filter);
            if (maxRecords != 0)
            {
                var results = await query.ToListAsync(cancellationToken);
                return results.Take(maxRecords);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}