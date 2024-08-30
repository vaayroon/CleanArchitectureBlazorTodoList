using TodoList.Application.Core.Abstractions.Messaging;
using TodoList.Application.DTOs;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Tasks.Queries.GetAllTasks;

internal sealed class GetAllTasksQueryHandler
    : IQueryHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(
        GetAllTasksQuery request,
        CancellationToken cancellationToken)
    {
        TodoTaskStatus? statusType;
        if (request.Status is not null)
        {
            var tempStatusType = TodoTaskStatus.FromName(request.Status);
            if (tempStatusType is null)
            {
                return Result.Failure<IEnumerable<TaskDto>>(new Error(
                    "InvalidStatus",
                    $"Invalid status: {request.Status}",
                    ErrorType.Validation));
            }
            statusType = tempStatusType;
        } else {
            statusType = TodoTaskStatus.All;
        }

        IEnumerable<TaskItem> tasks = await _taskRepository.GetAllAsync(statusType, cancellationToken);
        IEnumerable<TaskDto> taskDtos = tasks.Select(task => new TaskDto(){
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted});
        return Result.Successs(taskDtos);
    }
}
