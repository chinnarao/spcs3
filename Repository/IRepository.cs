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

        /// <summary>
        /// A navigation property for the entities the store contains.
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// Saves the current store.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Saves the current store as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new entity in a store.
        /// </summary>
        /// <param name="entity">The entity to create in the store.</param>
        /// <returns>A <see cref="RepositoryResult"/> of the query.</returns>
        RepositoryResult Create(TEntity entity);

        /// <summary>
        /// Creates a new entity in a store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="RepositoryResult"/> of the asynchronous query.</returns>
        Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates a entity in a store.
        /// </summary>
        /// <param name="entity">The entity to update in the store.</param>
        /// <returns>A <see cref="RepositoryResult"/> of the query.</returns>
        RepositoryResult Update(TEntity entity);

        /// <summary>
        /// Updates a entity in a store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="RepositoryResult"/> of the asynchronous query.</returns>
        Task<RepositoryResult> UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a entity from the store.
        /// </summary>
        /// <param name="entity">The entity to delete from the store.</param>
        /// <returns>A <see cref="RepositoryResult"/> of the query.</returns>
        RepositoryResult Delete(TEntity entity);

        /// <summary>
        /// Deletes a entity from the store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="RepositoryResult"/> of the asynchronous query.</returns>
        Task<RepositoryResult> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Finds an entity with the given primary key values. If an entity with the given primary key values
        /// is being tracked by the context, then it is returned immediately without making a request to the
        /// database. Otherwise, a query is made to the database for an entity with the given primary key values
        /// and this entity, if found, is attached to the context and returned. If no entity is found, then
        /// null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        TEntity FindByKey(params object[] keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If an entity with the given primary key values
        /// is being tracked by the context, then it is returned immediately without making a request to the
        /// database. Otherwise, a query is made to the database for an entity with the given primary key values
        /// and this entity, if found, is attached to the context and returned. If no entity is found, then
        /// null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindByKeyAsync(params object[] keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If an entity with the given primary key values
        /// is being tracked by the context, then it is returned immediately without making a request to the
        /// database. Otherwise, a query is made to the database for an entity with the given primary key values
        /// and this entity, if found, is attached to the context and returned. If no entity is found, then
        /// null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindByKeyAsync(
            object[] keyValues,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a List of all records from an IQueryable
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> of all records from an IQuerable.</returns>
        IEnumerable<TEntity> All();

        /// <summary>
        /// Creates a List of all records from an IQueryable by enumerating it asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TEntity}"/> of all records from an IQuerable.
        /// </returns>
        Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}