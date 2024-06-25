using MassTransit;
using MongoDB.Driver;
using Play.Catalog.Service.Emtties;
using Play.Catalog.Service.Settings;
using Play.Common;
using Play.Common.MongoDb;
using Play.Common.Settings;

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

    public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        services.AddMassTransit(opt =>
        {
            opt.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                configurator.Host(rabbitMqSettings.Host);
                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
            });
        });

        return services;
    }
}
