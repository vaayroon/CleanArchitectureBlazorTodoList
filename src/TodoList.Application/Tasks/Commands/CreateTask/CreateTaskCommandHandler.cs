using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Application.DTOs;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Tasks.Commands.CreateTask;

internal sealed class CreateTaskCommandHandler
    : ICommandHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskDto>> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = TaskItem.Create(
            request.Title,
            request.Description,
            request.IsCompleted);

        await _taskRepository.CreateAsync(task, cancellationToken);
        return Result.Successs(
            new TaskDto(){
                Id = task.Id!,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted});
    }
}
