using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Tasks.Commands.TaskToggleCompletion;

internal sealed class TaskToggleCompletionCommandHandler
    : ICommandHandler<TaskToggleCompletionCommand>
{
    private readonly ITaskRepository _taskRepository;

    public TaskToggleCompletionCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<Result> Handle(
        TaskToggleCompletionCommand request,
        CancellationToken cancellationToken)
    {
        TaskItem? taskItem = await _taskRepository.GetByIdAsync(
            request.Id,
            cancellationToken);
        if (taskItem is null)
        {
            return Result.Failure(
                new Error(
                    "404",
                    $"Task with id {request.Id} not found",
                    ErrorType.NotFound));
        }

        return await taskItem.ToggleCompletionAsync(
            request.MarkAsComplete!.Value,
            _taskRepository,
            cancellationToken);
    }
}
