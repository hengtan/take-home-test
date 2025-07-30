using Fundo.Application.Common.Errors;

namespace Fundo.Application.Common.Results;

public sealed class Result<T>
{
    public T? Value { get; }
    public Error? Error { get; }
    public bool IsSuccess => Error is null;
    public bool IsFailure => !IsSuccess;

    private Result(T? value, Error? error)
    {
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value, null);
    public static Result<T> Failure(Error error) => new(default, error);

    public Result<K> Map<K>(Func<T, K> func) =>
        IsSuccess ? Result<K>.Success(func(Value!)) : Result<K>.Failure(Error!);
}