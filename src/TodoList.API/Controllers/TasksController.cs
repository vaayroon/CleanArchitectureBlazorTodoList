using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.DTOs;
using TodoList.Application.Tasks.Commands.DeleteTask;
using TodoList.Application.Tasks.Commands.TaskToggleCompletion;
using TodoList.Application.Tasks.Queries.GetAllTasks;
using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ISender _sender;

    public TasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks(
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(new GetAllTasksQuery(status))
            .Bind(query => _sender.Send(query, cancellationToken))
            .MatchResult<IEnumerable<TaskDto>, IActionResult>(
                Ok,
                BadRequest);
    }

    [HttpPatch("{id}/toggle-completion")]
    public async Task<IActionResult> ToggleCompletion(
        [FromRoute] string id,
        [FromBody] TaskToggleCompletionRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.ToCommand(id))
            .Bind(command => _sender.Send(command, cancellationToken))
            .MatchResult<IActionResult>(
                result => Ok(result.Success),
                result => BadRequest(result is ICustomValidationResult validationError ? validationError.Errors : result.Error));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(
        [FromBody]TaskDto taskDto,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(taskDto.ToCreateTaskCommand())
            .Bind(command => _sender.Send(command, cancellationToken))
            .MatchResult<TaskDto, IActionResult>(
                Ok,
                result => BadRequest(result is ICustomValidationResult validationError ? validationError.Errors : result.Error));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] string id,
        [FromBody] TaskDto taskDto,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(taskDto.ToUpdateTaskCommand(id))
            .Bind(command => _sender.Send(command, cancellationToken))
            .MatchResult<IActionResult>(
                result => Ok(result.Success),
                result => BadRequest(result is ICustomValidationResult validationError ? validationError.Errors : result.Error));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(new DeleteTaskCommand(id))
            .Ensure(command => !string.IsNullOrWhiteSpace(command.Id), Error.NullValue)
            .Bind(command => _sender.Send(command, cancellationToken))
            .MatchResult<IActionResult>(
                result => Ok(result.Success),
                result => BadRequest(result.Error));
    }


}
