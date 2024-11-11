using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StickyTunes.Business.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = new Dictionary<string, string[]>();

            foreach (var entry in context.ModelState)
            {
                var fieldName = entry.Key;
                
                var errorMessages = entry.Value.Errors
                    .Select(error => error.ErrorMessage)
                    .ToArray();

                if (errorMessages.Length > 0)
                {
                    errors.Add(fieldName, errorMessages);
                }
            }

            var responseObj = new
            {
                message = "Validation failed",
                errors = errors
            };

            context.Result = new BadRequestObjectResult(responseObj);

            return;
        }
        
        await next();
    }
}