using TodoList.Application.Core.Abstractions.Messaging;

namespace TodoList.Application.Tasks.Commands.DeleteTask;

public sealed record DeleteTaskCommand(
    string Id) : ICommand;
