using RuiSantos.AutoFill.Application.Factories;

namespace RuiSantos.AutoFill.Application.Core;

internal static class Asserts
{
    public static void ValidateFileExists(string filePath)
    {
        if (!File.Exists(filePath))
            throw ExceptionsFactory.FileNotFound(filePath);
    }
}