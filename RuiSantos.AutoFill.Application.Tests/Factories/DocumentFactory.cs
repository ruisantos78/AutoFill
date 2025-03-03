namespace RuiSantos.AutoFill.Application.Tests.Factories;

internal static class DocumentFactory
{
    public static async Task<string> Create(string documentName)
    {
        var assembly = typeof(DocumentFactory).Assembly;
        var resourceName = assembly.GetManifestResourceNames().First(str => str.EndsWith($"Resources.Documents.{documentName}"));
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream == null)
            return string.Empty;
        
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}

internal static class Documents
{
    public const string AdEtExtraJudicia = "AdEtExtraJudicia";
}