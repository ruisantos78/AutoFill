namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

/// <summary>
/// Interface for engine client.
/// </summary>
public interface IEngineClient
{
    /// <summary>
    /// Executes a prompt and returns the response as a string.
    /// </summary>
    /// <param name="prompt">The prompt to execute.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response as a string.</returns>
    Task<string?> ExecutePromptAsync(string prompt);

    /// <summary>
    /// Executes a prompt and returns the response as a specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="prompt">The prompt to execute.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response as the specified type.</returns>
    Task<TResponse?> ExecutePromptAsync<TResponse>(string prompt) where TResponse : class;

    /// <summary>
    /// Uploads a file and executes a prompt, returning the response as a string.
    /// </summary>
    /// <param name="prompt">The prompt to execute.</param>
    /// <param name="fileName">The file name to upload.</param>
    /// <param name="stream">The file data to upload.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response as a string.</returns>
    Task<string> UploadFileAndExecuteAsync(string prompt, string fileName, Stream stream);
}