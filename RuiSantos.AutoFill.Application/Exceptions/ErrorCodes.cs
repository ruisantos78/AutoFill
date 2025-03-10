namespace RuiSantos.AutoFill.Application.Exceptions;

/// <summary>
/// Provides error codes for exceptions thrown by the application.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Error code indicating an unexpected error.
    /// </summary>
    public const int UnexpectedError = -1;

    
    /// <summary>
    /// Error code indicating that fields are not detected by the engine.
    /// </summary>
    public const int FieldAreNotDetectByEngine = 1;

    /// <summary>
    /// Error code indicating that the Markdown conversion failed.
    /// </summary>
    public const int MarkdownConversionFailed = 2;
}