using FluentValidation;

namespace TodoList.Application.Tasks.Commands.UpdateTask;

internal sealed class UpdateTaskCommandValidator
    : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
