using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Application.DTOs;
using TodoList.Domain.SharedKernel.Constants;

namespace TodoList.Application.Tasks.Queries.GetAllTasks;

public sealed record GetAllTasksQuery(
    string? Status)
    : IQuery<IEnumerable<TaskDto>>;
