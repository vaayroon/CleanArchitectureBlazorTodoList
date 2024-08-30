using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Domain.SharedKernel.Extensions;

public sealed class CustomValidationResult<TValue>
    : Result<TValue>, ICustomValidationResult
{
    public CustomValidationResult(Error[] errors)
        : base(default, false, ICustomValidationResult.ValidationError) =>
        Errors = errors;

    public new Error[] Errors { get; }

    public static CustomValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}
