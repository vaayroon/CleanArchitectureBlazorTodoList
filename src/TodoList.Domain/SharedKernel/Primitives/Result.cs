using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Domain.SharedKernel.Primitives;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        Errors = new Error[] { error };
        Success = Success.None;
    }

    protected internal Result(bool isSuccess, Error[] errors)
    {
        if (isSuccess || errors.Length <= 0)
        {
            throw new ArgumentException("Invalid error", nameof(errors));
        }

        IsSuccess = isSuccess;
        Error = errors[0];
        Errors = errors;
        Success = Success.None;
    }

    protected internal Result(bool isSuccess, Success success)
    {
        if (isSuccess && success == Success.None
            || !isSuccess && success != Success.None)
        {
            throw new ArgumentException("Invalid success", nameof(success));
        }

        IsSuccess = isSuccess;
        Error = Error.None;
        Errors = new Error[] { Error };
        Success = success;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public Error[] Errors { get; }

    public Success Success { get; }

    public static Result Successs() => new(true, Error.None);

    // My Temporal solution
    public static Result Successs(Success success) => new(true, success);

    // My Temporal solution
    public static Result<TValue> Successs<TValue>(TValue value, Success success) => new(value, true, success);

    public static Result<TValue> Successs<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Create() => Successs();

    public static Result<TValue> Create<TValue>(TValue? value)
        where TValue : class =>
            value is null ? Failure<TValue>(Error.NullValue) : Successs(value);

    public static Result<TValue> Create<TValue>(TValue? value, Error error)
        where TValue : class =>
            value is null ? Failure<TValue>(error) : Successs(value);

    public static Result Failure(Error error) => new(false, error);

    public static Result Failure(Error[] errors) => new(false, errors);

    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);

    public static Result<TValue> Failure<TValue>(Error[] errors) => new(default!, false, errors);

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Successs();
    }

    public static Result<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        return predicate(value) ?
            Successs(value) :
            Failure<T>(error);
    }

    public static Result<T> Ensure<T>(
        T value,
        params (Func<T, bool> Predicate, Error Error)[] functions)
    {
        var results = new List<Result<T>>();
        foreach ((Func<T, bool> Predicate, Error Error) function in functions)
        {
            results.Add(Ensure(value, function.Predicate, function.Error));
        }

        return Combine(results.ToArray());
    }

    public static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (Array.Exists(results, result => result.IsFailure))
        {
            return Failure<T>(
                results
                    .SelectMany(result => result.Errors)
                    .Where(error => error != Error.None)
                    .Distinct()
                    .ToArray());
        }

        return Successs(results[0].Value);
    }
}
