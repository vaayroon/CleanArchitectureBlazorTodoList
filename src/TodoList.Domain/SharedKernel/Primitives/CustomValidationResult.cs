namespace TodoList.Domain.SharedKernel.Primitives;

public sealed class CustomValidationResult
    : Result, ICustomValidationResult
{
    public CustomValidationResult(Error[] errors)
        : base(false, ICustomValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public new Error[] Errors { get; }

    public static CustomValidationResult WithErrors(Error[] errors) => new(errors);
}

