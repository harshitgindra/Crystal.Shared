using System.Linq;
using Crystal.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Crystal.EntityFrameworkCore
{
    /// <summary>
    /// Service collection extension to configure unit of work
    /// </summary>
    public static class UnitOfWorkServiceCollectionExtension
    {
        /// <summary>
        /// Configures unit of work repositories in the service collection to support the implementation
        /// Make sure that the dB context is registered before registering unit of work repositories
        /// </summary>
        /// <typeparam name="TContext">Derived class of BaseContext</typeparam>
        /// <param name="serviceCollection">Service collection</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureUnitOfWork<TContext>(this IServiceCollection serviceCollection)
            where TContext : BaseContext
        {
            //***
            //*** Register unit of work interface and repositories
            //***
            serviceCollection.AddTransient<IBaseUowRepository, BaseUowRepository>();
            //***
            //*** Register dBContext if not registered in service collection
            //***
            if (!serviceCollection.Any(x => x.ServiceType == typeof(TContext)))
            {
                serviceCollection.AddDbContext<TContext>();
            }
            //***
            //*** Map base context to the context used in the application
            //***
            serviceCollection.AddTransient<BaseContext>(x =>
                serviceCollection.BuildServiceProvider().GetService<TContext>());

            return serviceCollection;
        }
    }
}
