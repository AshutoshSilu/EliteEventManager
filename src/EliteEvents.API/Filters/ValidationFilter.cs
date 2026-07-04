using EliteEvents.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EliteEvents.API.Filters;

/// <summary>
/// Action filter that validates ModelState and returns a structured error response.
/// </summary>
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(x => x.ErrorMessage))
                .ToList();

            var response = ApiResponse.FailResponse("Validation failed.", errors);
            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
