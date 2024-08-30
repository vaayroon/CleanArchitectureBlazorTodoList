using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Domain.Entities;

public class TaskItem
{
    private TaskItem(string title, string? description, bool isCompleted)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
    }

    public TaskItem()
    {
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = default;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public async Task<Result> ToggleCompletionAsync(
        bool markAsComplete,
        ITaskRepository taskRepository,
        CancellationToken cancellationToken)
    {
        // Check if is in the same state
        if (IsCompleted == markAsComplete)
        {
            return Result.Failure(
                new Error(
                    "Task.IsCompleted",
                    "Task is already in the requested state",
                    ErrorType.Problem));
        }

        IsCompleted = markAsComplete;
        await taskRepository.UpdateAsync(this, cancellationToken);
        return Result.Successs(
            new Success(
                "Task.ToggleCompletion",
                "Task completion status toggled successfully"));
    }

    public static TaskItem Create(
        string title,
        string? description,
        bool isCompleted)
    {
        return new(title, description, isCompleted);
    }

    public Result UpdateTask(
        string title,
        string? description,
        bool isCompleted)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure(
                new Error(
                    "Task.Title",
                    "Title cannot be empty or whitespace",
                    ErrorType.Problem));
        }

        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        return Result.Successs(
            new Success(
                "Task.Update",
                "Task updated successfully"));
    }
}
