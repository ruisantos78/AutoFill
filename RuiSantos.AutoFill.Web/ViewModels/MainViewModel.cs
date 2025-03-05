using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;

namespace RuiSantos.AutoFill.Web.ViewModels;

public class MainViewModel(
    ISnackbar snackbar,
    ILogger<MainViewModel> logger,
    IGenerateTemplateService generateTemplateService)
{
    private const int MaxFileSize = 10_485_760;
    
    public readonly List<TemplateDocument> TemplateDocuments = [];
    
    public async void UploadTemplateFile(IBrowserFile? file)
    {
        try
        {
            if (file is null)
                return;

            await using var stream = file.OpenReadStream(MaxFileSize);
            await GenerateTemplateAsync(file.Name, stream);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error on Generate template file {fileName}", file?.Name);
            snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task GenerateTemplateAsync(string fileName, Stream fileStream)
    {
        logger.LogInformation("Generate template file {fileName}", fileName);
        try
        {
            var template = await generateTemplateService.ExtractFromFileAsync(fileName, fileStream);
            TemplateDocuments.Add(template);

            snackbar.Add($"Template generate with success: {fileName}", Severity.Success);
        }
        catch (ApplicationServiceException asex)
        {
            logger.LogWarning(asex, "Fail to Generate template from file {fileName}", fileName);
            snackbar.Add(asex.Message, Severity.Warning);
        }
    }
}