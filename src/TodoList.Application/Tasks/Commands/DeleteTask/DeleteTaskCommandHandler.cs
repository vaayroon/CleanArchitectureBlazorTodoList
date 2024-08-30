using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Tasks.Commands.DeleteTask;

internal sealed class DeleteTaskCommandHandler
    : ICommandHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        TaskItem? task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            return Result.Failure(
                new Error(
                    "Task.Delete",
                    "Task not found",
                    ErrorType.NotFound));
        }

        await _taskRepository.DeleteAsync(request.Id);
        return Result.Successs();
    }
}
