namespace RuiSantos.AutoFill.Infrastructure.Engines.Factories;

/// <summary>
/// Factory class for creating prompt-related resources.
/// </summary>
internal static class PromptFactory
{
    /// <summary>
    /// Asynchronously retrieves the content of an embedded resource as a string.
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the content of the resource as a string.</returns>
    /// <exception cref="Exception">Thrown when the specified resource is not found.</exception>
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

    /// <summary>
    /// Asynchronously retrieves the content of the 'DetectFieldsAndValues' prompt resource and concatenates it with the provided document.
    /// </summary>
    /// <param name="document">The document to concatenate with the prompt resource.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the concatenated string.</returns>
    public static async Task<string> GetDetectFieldsAndValuesAsync(string document)
    {
        var resource = await GetResourceStringAsync("Resources.Prompts.DetectFieldsAndValues");
        return string.Concat(resource, document);
    }
}