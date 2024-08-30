using System;
using TodoList.Domain.Entities;
using TodoList.Domain.SharedKernel.Constants;

namespace TodoList.Domain.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync(
        TodoTaskStatus todoTaskStatus,
        CancellationToken cancellationToken = default);

    Task<TaskItem?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string id);
}
