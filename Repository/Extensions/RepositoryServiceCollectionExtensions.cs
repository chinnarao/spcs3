using Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring respository services.
    /// </summary>
    public static class RepositoryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default repository system configuration for the specified entity and context types.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <returns>The collection of services available in the application.</returns>
        public static IServiceCollection AddRepository(
            this IServiceCollection services)
        {
            services.TryAddScoped(
                typeof(IRepository<,>),
                typeof(Repository<,>));

            return services;
        }
    }
}