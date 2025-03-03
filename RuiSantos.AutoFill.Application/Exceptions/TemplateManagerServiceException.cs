namespace RuiSantos.AutoFill.Application.Exceptions;

public class TemplateManagerServiceException(int erroCode, string action, string message) : Exception(message)
{
    public int ErrorCode { get; } = erroCode;
    public string Action { get; } = action;
}