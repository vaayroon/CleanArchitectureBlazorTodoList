using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TodoList.Application.Core.Abstractions.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger;

    public ValidationPipelineBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ValidationFailure[] validationFailures = await ValidateAsync(request);

        if (validationFailures.Length == 0)
        {
            _logger.LogInformation(
                "Completed validation request: {@RequestType}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow);
            return await next();
        }

        _logger.LogError(
            "Failure validation request {@RequestType}, {@ValidationErrors}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            validationFailures,
            DateTime.UtcNow);

        Error[] validationErrors = CreateValidationError(validationFailures);

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            object customValidationResult = typeof(CustomValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResponse).GenericTypeArguments[0])
                .GetMethod(nameof(CustomValidationResult.WithErrors))!
                .Invoke(null, [validationErrors])!;
            return (TResponse)customValidationResult;
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            object customValidationResult = CustomValidationResult.WithErrors(validationErrors);
            return (TResponse)customValidationResult;
        }

        throw new ValidationException(validationFailures);
    }

    private static Error[] CreateValidationError(ValidationFailure[] validationFailures) =>
        validationFailures.Select(f => Error.Problem(f.PropertyName, f.ErrorMessage)).ToArray();

    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        if (!_validators.Any())
        {
            return [];
        }

        _logger.LogInformation(
            "Starting validation request: {@RequestType}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }
}
