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
            return new MongoRepository<Item>(database, $"{nameof(Item)}s");
        });
        return services;
    }

    public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseCors(builder =>
        {
            builder.WithOrigins(configuration["AllowedOrigin"])
                     .AllowAnyHeader()
                     .AllowAnyMethod();
        });
        return app;
    }
}
