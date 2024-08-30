using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IMongoClient mongoClient)
    {
        string databaseName = "CleanArchitectureBlazorTodoList";
        string collectionName = "Tasks";
        IAsyncCursor<string> databaseList = await mongoClient.ListDatabaseNamesAsync();
        List<string> databases = await databaseList.ToListAsync();
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        if (!databases.Contains(databaseName))
        {
            await database.CreateCollectionAsync(collectionName);
        }

        IMongoCollection<TaskItem> collection = database.GetCollection<TaskItem>(collectionName);

        IAsyncCursor<string> collectionList = await database.ListCollectionNamesAsync();
        List<string> collections = await collectionList.ToListAsync();
        if (!collections.Contains(collectionName))
        {
            await database.CreateCollectionAsync(collectionName);
        }

        List<TaskItem> existingTasks = await (await collection.FindAsync(_ => true)).ToListAsync();
        if (existingTasks.Count == 0)
        {
            TaskItem[] sampleTasks = new[]
            {
                new TaskItem
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Sample Task completed",
                    Description = "This is a sample completed task description",
                    IsCompleted = true
                },
                new TaskItem
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Sample Task not completed",
                    Description = "This is a sample not completed task description",
                    IsCompleted = false
                }
            };

            await collection.InsertManyAsync(sampleTasks);
        }
    }

    public static void SeedCollection(this IServiceProvider serviceProvider)
    {
        IMongoClient mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        SeedAsync(mongoClient).GetAwaiter().GetResult();
    }
}
