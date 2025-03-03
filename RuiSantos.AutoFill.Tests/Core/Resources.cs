namespace RuiSantos.AutoFill.Tests.Core;

internal static class Resources
{
    public static async Task<string> GetStringAsync(string resourceName)
    {
        var assembly = typeof(Resources).Assembly;
        var fullResourceName = assembly.GetManifestResourceNames().First(str => str.EndsWith(resourceName));
        await using var stream = assembly.GetManifestResourceStream(fullResourceName);
        
        if (stream == null)
            return string.Empty;
        
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}