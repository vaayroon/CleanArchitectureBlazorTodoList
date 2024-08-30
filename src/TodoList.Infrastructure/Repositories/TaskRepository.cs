using System;
using MongoDB.Driver;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;

namespace TodoList.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IMongoCollection<TaskItem> _tasks;

    public TaskRepository(IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase("CleanArchitectureBlazorTodoList");
        _tasks = database.GetCollection<TaskItem>("Tasks");
    }

    public async Task CreateAsync(
        TaskItem task,
        CancellationToken cancellationToken = default) =>
        await _tasks.InsertOneAsync(task, cancellationToken: cancellationToken);

    public async Task DeleteAsync(string id) => await _tasks.DeleteOneAsync(t => t.Id == id);

    public async Task<IEnumerable<TaskItem>> GetAllAsync(
        TodoTaskStatus todoTaskStatus,
        CancellationToken cancellationToken = default)
    {

        IAsyncCursor<TaskItem> response = await _tasks.FindAsync(todoTaskStatus.Filter, cancellationToken: cancellationToken);
        return await response.ToListAsync(cancellationToken);
    }

    

    public async Task<TaskItem?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        IAsyncCursor<TaskItem> response = await _tasks.FindAsync(t => t.Id == id, cancellationToken: cancellationToken);
        return await response.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        TaskItem task,
        CancellationToken cancellationToken = default) =>
        await _tasks.ReplaceOneAsync(t => t.Id == task.Id, task, cancellationToken: cancellationToken);
}
