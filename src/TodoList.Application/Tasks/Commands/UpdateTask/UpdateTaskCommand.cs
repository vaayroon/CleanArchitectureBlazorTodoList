using TodoList.Application.Core.Abstractions.Messaging;

namespace TodoList.Application.Tasks.Commands.UpdateTask;

public sealed record UpdateTaskCommand(
    string Id,
    string Title,
    string? Description,
    bool IsCompleted) : ICommand;
