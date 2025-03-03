using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RuiSantos.AutoFill.App.Services;

namespace RuiSantos.AutoFill.App.ViewModels;

public partial class MainViewModel(
    IMessenger messenger,
    ITemplateServices templateServices
) : ObservableRecipient, 
    IRecipient<UploadTemplateDocumentCompleted>,
    IRecipient<UploadTemplateDocumentFail>
{
    [ObservableProperty] string _geminiApiKey = string.Empty;
    
    partial void OnGeminiApiKeyChanged(string value)
    {
        Preferences.Set("Engine:Gemini:ApiKey", value);
    }
    
    [RelayCommand]
    private async Task UploadFileAsync()
    {
        var folderResult = await FolderPicker.Default.PickAsync();
        if (folderResult.Folder is not null)
        {
            var fileName = Directory.GetFiles(folderResult.Folder.Path, "*.pdf").FirstOrDefault();
            if (fileName is null)
                return;
            
            await templateServices.UploadFileAsync(fileName);
        }
    }

    protected override void OnActivated()
    {
        messenger.RegisterAll(this);
    }

    public void Receive(UploadTemplateDocumentCompleted message)
    {
        if (message.Value is null)
            return; 
        
        Snackbar.Make($"Template {message.Value.Name} uploaded successfully").Show();
    }

    public void Receive(UploadTemplateDocumentFail message)
    {
        Snackbar.Make(message.Value).Show();
    }
}