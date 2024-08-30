using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Tasks.Commands.UpdateTask;

internal sealed class UpdateTaskCommandHandler
    : ICommandHandler<UpdateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<Result> Handle(
        UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        TaskItem? task = await _taskRepository.GetByIdAsync(command.Id, cancellationToken);
        if (task is null)
        {
            return Result.Failure(
                new Error(
                    "404",
                    "Task not found",
                    ErrorType.NotFound));
        }

        Result updateResult = task.UpdateTask(command.Title, command.Description, command.IsCompleted);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        await _taskRepository.UpdateAsync(task, cancellationToken);
        return updateResult;
    }
}
