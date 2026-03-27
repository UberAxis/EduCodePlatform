using System.Text.Json;

namespace EduCodePlatform.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,

                    KeyNotFoundException => StatusCodes.Status404NotFound,

                    InvalidOperationException => StatusCodes.Status409Conflict,

                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = ex.Message
                };

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
