using System.Net;
using System.Text.Json;
using LibraryManagement.API.Common;

namespace LibraryManagement.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

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
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.FailResponse(
                "An unexpected error occurred.",
                new List<string> { ex.Message } // consider hiding raw message in Production
            );

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
