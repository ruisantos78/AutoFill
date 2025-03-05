using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Web.ViewModels.Commons;

namespace RuiSantos.AutoFill.Web.ViewModels;

[RegisterService] public class MainViewModel(
    ISnackbar snackbar,
    ILogger<MainViewModel> logger,
    IGenerateTemplateService generateTemplateService)
: ObservableObject
{
    private const int MaxFileSize = 10_485_760;

    public List<TemplateDocument> TemplateDocuments { get; } = [];
    
    private bool _isProcessing;
    public bool IsProcessing 
    { 
        get => _isProcessing;
        set => SetProperty(ref _isProcessing, value);
    }
    
    public async void UploadTemplateFile(IBrowserFile? file)
    {
        try
        {
            if (file is null)
                return;

            IsProcessing = true;
            await using var stream = file.OpenReadStream(MaxFileSize);
            await GenerateTemplateAsync(file.Name, stream);
        }
        catch (Exception ex)
        {
            IsProcessing = false;
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
            IsProcessing = false;

            snackbar.Add($"Template generate with success: {fileName}", Severity.Success);
        }
        catch (ApplicationServiceException asex)
        {
            IsProcessing = false;
            
            logger.LogWarning(asex, "Fail to Generate template from file {fileName}", fileName);
            snackbar.Add(asex.Message, Severity.Warning);
        }
    }
}