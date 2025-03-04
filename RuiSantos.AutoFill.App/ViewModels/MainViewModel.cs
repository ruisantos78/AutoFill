using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Interfaces;

namespace RuiSantos.AutoFill.App.ViewModels;

public partial class MainViewModel(
    IMessenger messenger,
    IGenerateTemplateService generateTemplateServices
) : ObservableRecipient
{
    protected override void OnActivated()
    {
        messenger.RegisterAll(this);
    }

    [RelayCommand]
    private async Task UploadFileAsync()
    {
        var folderResult = await FolderPicker.Default.PickAsync();
        if (folderResult.Folder is null)
            return;

        var fileName = Directory.GetFiles(folderResult.Folder.Path, "*.pdf").FirstOrDefault();
        if (fileName is null)
            return;

        await GenerateNewTemplateFromFileAsync(fileName);
    }

    private async Task GenerateNewTemplateFromFileAsync(string fileName)
    {
        try
        {
            var template = await generateTemplateServices.ExtractFromFileAsync(fileName);
            await Snackbar.Make($"Template {template.Name} uploaded successfully").Show();
        }
        catch (ApplicationServiceException tsex)
        {
            await Snackbar.Make(tsex.Message).Show();
        }
        catch (Exception ex)
        {
            await Snackbar.Make(ex.Message).Show();
            throw;
        }
    }
}