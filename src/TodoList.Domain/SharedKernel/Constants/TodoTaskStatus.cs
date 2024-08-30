using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using TodoList.Domain.Entities;
using TodoList.Domain.SharedKernel.Primitives;

namespace TodoList.Domain.SharedKernel.Constants;

public sealed record TodoTaskStatus : SmartEnum<TodoTaskStatus>
{
    public static readonly TodoTaskStatus Pending = new(0, "pending", t => !t.IsCompleted);
    public static readonly TodoTaskStatus Completed = new(1, "completed", t => t.IsCompleted);
    public static readonly TodoTaskStatus All = new(2, "all", t => true);

    public TodoTaskStatus(int value, string name, Expression<Func<TaskItem, bool>> filter)
        : base(value, name)
    {
        Filter = filter;
    }

    public Expression<Func<TaskItem, bool>> Filter { get; init; }
}
