using TodoList.Application.Tasks.Commands.CreateTask;
using TodoList.Application.Tasks.Commands.UpdateTask;

namespace TodoList.Application.DTOs;

public sealed record TaskDto
{
    public string? Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsCompleted { get; init; }
    
    public CreateTaskCommand ToCreateTaskCommand() => new (Title, Description, IsCompleted);

    public UpdateTaskCommand ToUpdateTaskCommand(string id) => new (id, Title, Description, IsCompleted);
}
