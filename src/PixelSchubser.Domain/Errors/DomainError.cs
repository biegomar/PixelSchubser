namespace PixelSchubser.Domain.Errors;

public enum DomainErrorCategory
{
    Validation,
    NotFound,
    Conflict,
    Unexpected
}

public sealed record DomainError(string Code, string Message, DomainErrorCategory Category)
{
    public static DomainError Validation(string code, string message) =>
        new(code, message, DomainErrorCategory.Validation);

    public static DomainError NotFound(string code, string message) =>
        new(code, message, DomainErrorCategory.NotFound);

    public static DomainError Conflict(string code, string message) =>
        new(code, message, DomainErrorCategory.Conflict);

    public static DomainError Unexpected(string code, string message) =>
        new(code, message, DomainErrorCategory.Unexpected);
}
