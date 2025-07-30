namespace Fundo.Application.Common.Errors;

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Validation(string message) => new("Validation", message);
    public static Error NotFound(string message) => new("NotFound", message);
    public static Error Conflict(string message) => new("Conflict", message);
    public static Error Internal(string message) => new("Internal", message);
    public static Error Unexpected(string message) => new("Unexpected", message);
}