using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Emtties;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddMongodbSettings(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        services.AddSingleton(serviceProvider =>
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });
        return services;
    }
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
