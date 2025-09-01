using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.Result;

namespace NewerDown.Extensions;

public static class ResultExtensions
{
    public static IActionResult Match(
        this Result result,
        Func<IActionResult> onSuccess,
        Func<Error, IActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }
    
    public static IActionResult Match<T>(
        this Result<T> result,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }
    
    public static IActionResult ToDefaultApiResponse(this Result result)
    {
        return result.Match(
            onSuccess: () => new NoContentResult(),
            onFailure: error => new BadRequestObjectResult(new { Error = error })
        );
    }

    public static IActionResult ToDefaultApiResponse<T>(this Result<T> result)
    {
        return result.Match(
            onSuccess: value => new OkObjectResult(value),
            onFailure: error => new BadRequestObjectResult(new { Error = error })
        );
    }
}