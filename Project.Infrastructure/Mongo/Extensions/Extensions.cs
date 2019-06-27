using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Project.Common.Types;
using Project.Core;

namespace Project.Infrastructure.Mongo.Extensions
{
    public static class Extensions
    {
        public static void AddMongo(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            var mongoOptions = new MongoOptions();
            configuration.GetSection(MongoOptions.SectionName).Bind(mongoOptions);

            var mongoClient = new MongoClient(mongoOptions.ConnectionString);

            services.AddSingleton(mongoOptions);
            services.AddSingleton(mongoClient);

            services.AddTransient<IMongoDatabase>(s =>
            {
                var _mongoClient = s.GetRequiredService<MongoClient>();
                var _mongoOptions = s.GetRequiredService<MongoOptions>();
                return _mongoClient.GetDatabase(_mongoOptions.DataBase);
            });

            services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();
            services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();

        }

        public static void AddMongoRepository<TEntity>(this IServiceCollection services, string collectionName)
            where TEntity : IIdentifiable
        {
            services.AddTransient<IRepository<TEntity>>(s =>
            {
                var dbContext = s.GetRequiredService<IMongoDatabase>();
                return new MongoRepository<TEntity>(dbContext, collectionName);
            });
            
        }
    }
}
