using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;
using RuiSantos.AutoFill.Desktop.ViewModels.Commons;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;

namespace RuiSantos.AutoFill.Desktop.ViewModels;

public class TemplateFormsViewModel(
    ILogger<TemplateFormsViewModel> logger,
    IGenerateTemplateService generateTemplateService,
    ISnackbar snackbar)
    : IViewModel
{
    public async void OnFileUploaded(IBrowserFile? file)
    {
        try
        {
            if (file is not null)
                return;
        
            await generateTemplateService.ExtractFromFileAsync(file.Name, file.OpenReadStream())
                .ContinueWith(GenerateTemplateCreationCompleted);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating template");
            snackbar.Add("Error creating template", Severity.Error);
        }
    }

    private void GenerateTemplateCreationCompleted(Task<TemplateDocument> obj)
    {
        snackbar.Add("Template create with success!", Severity.Success);
    }
}