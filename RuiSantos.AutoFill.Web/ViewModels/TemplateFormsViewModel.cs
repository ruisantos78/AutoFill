using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Web.ViewModels.Commons;

namespace RuiSantos.AutoFill.Web.ViewModels;

public class TemplateFormsViewModel(
    ILogger<TemplateFormsViewModel> logger,
    IGenerateTemplateService generateTemplateService,
    ISnackbar snackbar)
    : IViewModel
{
    public void OnFileUploaded(IBrowserFile? file)
    {
        if (file is null)
            return;
        
        try
        {
            logger.LogInformation("Upload model for generate a Template.");
            
            using var stream = file.OpenReadStream();
            generateTemplateService.ExtractFromFileAsync(file.Name, stream)
                .ContinueWith(OnExtractTemplateFromFileCompleted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading template");
            snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private void OnExtractTemplateFromFileCompleted(Task<TemplateDocument> task)
    {
        snackbar.Add("Template create with success!", Severity.Success);
    }
}