namespace RuiSantos.AutoFill.Infrastructure.Engines.Factories;

internal static class PromptFactory
{
    private static async Task<string> GetResourceStringAsync(string resourceName)
    {
        var assembly = typeof(PromptFactory).Assembly;
        var resourceFullName = assembly.GetManifestResourceNames().First(str => str.EndsWith(resourceName));
            
        await using var stream = assembly.GetManifestResourceStream(resourceFullName);
        if (stream == null)
            throw new Exception($"Resource '{resourceName}' not found.");
            
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public static async Task<string> GetDetectFieldsAndValuesAsync(string document)
    {
        var resource = await GetResourceStringAsync("Resources.Prompts.DetectFieldsAndValues");
        return string.Concat(resource, document);
    }
}