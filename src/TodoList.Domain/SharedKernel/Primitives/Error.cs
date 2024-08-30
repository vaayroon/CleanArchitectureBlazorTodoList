using TodoList.Domain.SharedKernel.Constants;

namespace TodoList.Domain.SharedKernel.Primitives;

public sealed record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "Error.NullValue",
        "The specified result value is null.",
        ErrorType.Failure);

    public Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public static implicit operator string(Error error) => error.Code;

    public static implicit operator Result(Error error) => Result.Failure(error);

    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Problem(string code, string message) =>
        new(code, message, ErrorType.Problem);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
    }
}
