using FluentValidation;

namespace TodoList.Application.Tasks.Commands.TaskToggleCompletion;

internal sealed class TaskToggleCompletionCommandValidator
    : AbstractValidator<TaskToggleCompletionCommand>
{
    public TaskToggleCompletionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.MarkAsComplete)
            .NotNull();
    }
}
