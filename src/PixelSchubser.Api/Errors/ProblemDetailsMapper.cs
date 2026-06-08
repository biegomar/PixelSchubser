using Microsoft.AspNetCore.Mvc;
using PixelSchubser.Domain.Errors;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Api.Errors;

public sealed class ProblemDetailsMapper
{
    public IResult ToResult(DomainResult result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return ToProblem(result.Error!);
    }

    public IResult ToResult<T>(DomainResult<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return onSuccess(result.Value);
        }

        return ToProblem(result.Error!);
    }

    private static IResult ToProblem(DomainError error)
    {
        var status = error.Category switch
        {
            DomainErrorCategory.Validation => StatusCodes.Status400BadRequest,
            DomainErrorCategory.NotFound => StatusCodes.Status404NotFound,
            DomainErrorCategory.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = status
        });
    }
}
