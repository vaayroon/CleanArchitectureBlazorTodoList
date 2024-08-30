using MediatR;
using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Application.Core.Abstractions.Messaging;

public interface IQueryHandler<TQuery>
    : IRequestHandler<TQuery, Result>
        where TQuery : IQuery
{
}

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
{
}
