using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TWS.Core.DTOs.Response;

namespace TWS.API.Filters
{
    /// <summary>
    /// Action filter that automatically validates ModelState and returns
    /// standardized ApiResponse format for validation errors
    /// </summary>
    public class ValidateModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => new
                    {
                        Field = x.Key,
                        Error = string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage
                    }))
                    .ToList();

                var errorMessage = "Validation failed. Please check the request data.";

                var response = new ApiResponse<object>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = new
                    {
                        ValidationErrors = errors
                    },
                    StatusCode = 400
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }
    }
}