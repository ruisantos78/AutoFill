namespace RuiSantos.AutoFill.Application.Exceptions;

/// <summary>
/// Exception thrown by the Template Manager Service.
/// </summary>
/// <param name="erroCode">The error code associated with the exception.</param>
/// <param name="action">The action that caused the exception.</param>
/// <param name="message">The message describing the exception.</param>
public class TemplateManagerServiceException(int erroCode, string action, string message) : Exception(message)
{
    /// <summary>
    /// Gets the error code associated with the exception.
    /// </summary>
    public int ErrorCode { get; } = erroCode;

    /// <summary>
    /// Gets the action that caused the exception.
    /// </summary>
    public string Action { get; } = action;
}