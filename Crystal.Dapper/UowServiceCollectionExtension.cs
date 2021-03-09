using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Crystal.Dapper
{
    public static class UowServiceCollectionExtension
    {
        public static IServiceCollection ConfigureUnitOfWork2<TContext>(this IServiceCollection serviceCollection, string connectionString)
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
                serviceCollection.AddTransient<IDbConnection>(x => ActivatorUtilities.CreateInstance<TContext>(x, connectionString));
            }
            return serviceCollection;
        }
    }
}