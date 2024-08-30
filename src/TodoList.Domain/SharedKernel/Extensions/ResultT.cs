using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Domain.SharedKernel.Extensions;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    protected internal Result(TValue? value, bool isSuccess, Error[] errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    protected internal Result(TValue? value, bool isSuccess, Success success)
        : base(isSuccess, success)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException();

    public static implicit operator Result<TValue>(TValue value) => Successs(value);

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}
