using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;

namespace RuiSantos.AutoFill.App.Services;

public interface ITemplateServices
{
    Task UploadFileAsync(string filePath);
}

internal class TemplateServices(
    IMessenger messenger,
    ITemplateManagerService templateManager,
    ILogger<TemplateServices> logger)
    : ITemplateServices
{
    public async Task UploadFileAsync(string filePath)
    {
        logger.LogInformation("Uploading file {filePath}", filePath);
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
        
            var fileName = Path.GetFileNameWithoutExtension(filePath);
        
            var markdown = await templateManager.ConvertDocumentIntoMarkdownAsync(filePath);
            var template = await templateManager.GenerateTemplateFromDocumentAsync(fileName,markdown);
        
            logger.LogInformation("Template {templateName} uploaded successfully", template.Name);
            messenger.Send(new UploadTemplateDocumentCompleted(template));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error uploading file {filePath}", filePath);
            messenger.Send(new UploadTemplateDocumentFail(exception.Message));
        }
    }
}

public class UploadTemplateDocumentCompleted(TemplateDocument? value) : ValueChangedMessage<TemplateDocument?>(value);
public class UploadTemplateDocumentFail(string message) : ValueChangedMessage<string>(message);