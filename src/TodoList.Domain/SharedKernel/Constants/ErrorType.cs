namespace TodoList.Domain.SharedKernel.Constants;

/// <summary>
/// Represents the type of an error.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Represents a failure.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// Represents a validation error.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// Represents a problem.
    /// </summary>
    Problem = 2,

    /// <summary>
    /// Represents a not found error.
    /// </summary>
    NotFound = 3,

    /// <summary>
    /// Represents a conflict error.
    /// </summary>
    Conflict = 4
}
