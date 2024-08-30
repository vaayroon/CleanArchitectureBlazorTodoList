using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TodoList.Domain.Repositories;
using TodoList.Infrastructure.Repositories;

namespace TodoList.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetConnectionString("MongoDb")));
        services.AddScoped<ITaskRepository, TaskRepository>();
        // services.AddScoped<TaskService>();

        services
            .BuildServiceProvider()
            .SeedCollection();

        return services;
    }
}
