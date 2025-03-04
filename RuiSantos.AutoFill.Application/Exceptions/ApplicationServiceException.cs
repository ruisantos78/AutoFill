namespace RuiSantos.AutoFill.Application.Exceptions;

/// <summary>
/// Exception thrown by the Application's Services.
/// </summary>
public class ApplicationServiceException : Exception
{
    /// <summary>
    /// Gets the error code associated with the exception.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Gets the action that caused the exception.
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationServiceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="action">The action that caused the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ApplicationServiceException(string action, Exception innerException) 
        : base("An unexpected error occurs.", innerException)
    {
        Action = action;
        ErrorCode = ErrorCodes.UnexpectedError;
    }

    /// <summary>
    /// Exception thrown by the Template Manager Service.
    /// </summary>
    /// <param name="erroCode">The error code associated with the exception.</param>
    /// <param name="action">The action that caused the exception.</param>
    /// <param name="message">The message describing the exception.</param>
    public ApplicationServiceException(int erroCode, string action, string message) : base(message)
    {
        Action = action;
        ErrorCode = erroCode;
    }
}