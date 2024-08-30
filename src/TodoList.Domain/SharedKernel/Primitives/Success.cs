namespace TodoList.Domain.SharedKernel.Primitives;

public sealed record Success
{
    public static readonly Success None = new(string.Empty, string.Empty);
    public static readonly Success NullValue = new(
        "Success.NullValue",
        "The specified result value is null.");

    public Success(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Success success) => success.Code;

    public static implicit operator Result(Success success) => Result.Successs(success);

    public bool Equals(Success? other)
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

