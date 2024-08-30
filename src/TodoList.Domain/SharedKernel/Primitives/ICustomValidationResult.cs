using TodoList.Domain.SharedKernel.Constants;

namespace TodoList.Domain.SharedKernel.Primitives;

public interface ICustomValidationResult
{
    public static readonly Error ValidationError = new(
        "Validation Error",
        "One or more validation errors occurred.",
        ErrorType.Validation);

    Error[] Errors { get; }
}
