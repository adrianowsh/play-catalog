using MongoDB.Driver;
using Play.Catalog.Service.Emtties;
using Play.Common;
using Play.Common.MongoDb;

namespace Play.Catalog.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<Item>>(static serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<Item>(database);
        });
        return services;
    }
}
