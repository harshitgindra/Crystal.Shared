using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Crystal.Dapper
{
    /// <summary>
    /// Service collection extensions to configure unit of work
    /// </summary>
    public static class UowServiceCollectionExtension
    {
        /// <summary>
        /// Configure unit of work needed to use crystal implementations for dapper
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureUnitOfWork<TContext>(this IServiceCollection serviceCollection, string connectionString)
            where TContext : DbConnection
        {
            //***
            //*** Register unit of work interface and repositories
            //***
            serviceCollection.AddTransient<IBaseUowRepository, BaseUowRepository>();
            //***
            //*** Register dBConnection if not registered in service collection
            //***
            if (!serviceCollection.Any(x => x.ServiceType == typeof(IDbConnection)))
            {
                serviceCollection.AddTransient<IDbConnection>(x => 
                    ActivatorUtilities.CreateInstance<TContext>(x, connectionString));
            }
            return serviceCollection;
        }
    }
}