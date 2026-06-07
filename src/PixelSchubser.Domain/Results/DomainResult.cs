using PixelSchubser.Domain.Errors;

namespace PixelSchubser.Domain.Results;

public class DomainResult
{
    protected DomainResult(bool isSuccess, DomainError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError? Error { get; }

    public static DomainResult Success() => new(true, null);

    public static DomainResult Failure(DomainError error) => new(false, error);
}

public sealed class DomainResult<T> : DomainResult
{
    private DomainResult(T? value, bool isSuccess, DomainError? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static DomainResult<T> Success(T value) => new(value, true, null);

    public static new DomainResult<T> Failure(DomainError error) => new(default, false, error);
}
