using FluentValidation;

namespace TodoList.Application.Tasks.Commands.CreateTask;

internal sealed class CreateTaskCommandValidator
    : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
