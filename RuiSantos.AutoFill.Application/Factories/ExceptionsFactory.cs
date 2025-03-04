using System.Runtime.CompilerServices;
using RuiSantos.AutoFill.Application.Exceptions;

namespace RuiSantos.AutoFill.Application.Factories;

internal static class ExceptionsFactory
{
    public static ApplicationServiceException UnexpectedError(Exception innerException, [CallerMemberName] string action = "")
        => new(action, innerException);
    
    public static ApplicationServiceException FieldsAreNotDetectedByEngine([CallerMemberName] string action = "")
        => new(ErrorCodes.FieldAreNotDetectByEngine, action, "Fields are not detected by the engine.");
    
    public static ApplicationServiceException MarkdownConversionFailed(string filePath, [CallerMemberName] string action = "")
        => new(ErrorCodes.MarkdownConversionFailed, action, $"The conversion of the file {filePath} to Markdown failed.");
}