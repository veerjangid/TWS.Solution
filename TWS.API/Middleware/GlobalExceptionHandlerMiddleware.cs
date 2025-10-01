using System.Net;
using System.Text.Json;
using TWS.Core.DTOs.Response;

namespace TWS.API.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns standardized ApiResponse error format
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = GetStatusCodeAndMessage(exception);
            context.Response.StatusCode = (int)statusCode;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null,
                StatusCode = (int)statusCode
            };

            // Include detailed error information only in Development environment
            if (_environment.IsDevelopment())
            {
                response.Data = new
                {
                    ExceptionType = exception.GetType().Name,
                    ExceptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                };
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

        private (HttpStatusCode statusCode, string message) GetStatusCodeAndMessage(Exception exception)
        {
            return exception switch
            {
                // Custom exceptions
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    validationEx.Message
                ),
                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    notFoundEx.Message
                ),
                UnauthorizedException unauthorizedEx => (
                    HttpStatusCode.Unauthorized,
                    unauthorizedEx.Message
                ),
                ForbiddenException forbiddenEx => (
                    HttpStatusCode.Forbidden,
                    forbiddenEx.Message
                ),
                BusinessRuleException businessRuleEx => (
                    HttpStatusCode.BadRequest,
                    businessRuleEx.Message
                ),
                InvalidOperationException invalidOpEx => (
                    HttpStatusCode.BadRequest,
                    invalidOpEx.Message
                ),
                ArgumentNullException argNullEx => (
                    HttpStatusCode.BadRequest,
                    $"Required parameter '{argNullEx.ParamName}' is missing"
                ),
                ArgumentException argEx => (
                    HttpStatusCode.BadRequest,
                    argEx.Message
                ),
                KeyNotFoundException => (
                    HttpStatusCode.NotFound,
                    "The requested resource was not found"
                ),
                // Default case
                _ => (
                    HttpStatusCode.InternalServerError,
                    _environment.IsDevelopment()
                        ? exception.Message
                        : "An internal server error occurred. Please contact support if the problem persists."
                )
            };
        }
    }

    // Custom exception classes
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }

    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}