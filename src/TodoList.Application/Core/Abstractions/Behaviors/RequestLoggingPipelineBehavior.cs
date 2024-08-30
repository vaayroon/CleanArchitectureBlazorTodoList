using System;
using TodoList.Domain.SharedKernel.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace TodoList.Application.Core.Abstractions.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(
        ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        _logger.LogInformation(
            "Starting request: {@RequestType}, {@DateTimeUtc}",
            requestName,
            DateTime.UtcNow);

        TResponse result = await next();

        if (result.IsFailure)
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError(
                    "Failure request: {@RequestType}, {@Error}, {@DateTimeUtc}",
                    requestName,
                    result.Error,
                    DateTime.UtcNow);
            }
        }
        else
        {
            _logger.LogInformation(
                "Completed request: {@RequestType}, {@DateTimeUtc}",
                requestName,
                DateTime.UtcNow);
        }

        return result;
    }
}
