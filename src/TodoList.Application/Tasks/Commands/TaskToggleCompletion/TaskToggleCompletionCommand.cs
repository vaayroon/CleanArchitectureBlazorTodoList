using TodoList.Application.Core.Abstractions.Messaging;

namespace TodoList.Application.Tasks.Commands.TaskToggleCompletion;

public sealed record TaskToggleCompletionCommand(
    string Id,
    bool? MarkAsComplete) : ICommand;
