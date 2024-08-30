namespace TodoList.Application.Tasks.Commands.TaskToggleCompletion;

public sealed record TaskToggleCompletionRequest
{
    public bool? MarkAsComplete { get; init; }

    public TaskToggleCompletionCommand ToCommand(string id)
    {
        return new TaskToggleCompletionCommand(id, MarkAsComplete);
    }
}
