using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Application.DTOs;

namespace TodoList.Application.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand(
    string Title,
    string? Description,
    bool IsCompleted) : ICommand<TaskDto>;
