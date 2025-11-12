using System.Data.Common;
using System.Net;
using System.Text.Json;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Api.Middlewares;

public class UnexpectedExceptionMiddleware(RequestDelegate next, ILogger<UnexpectedExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the exception for monitoring/debugging
            logger.LogError(ex, "Unhandled unexpected exception occurred");

            context.Response.ContentType = "application/json";

            // Detect DbException or TimeoutException wrapped inside InvalidOperationException
            Exception actualException = ex;
            while (actualException is InvalidOperationException && actualException.InnerException != null)
            {
                actualException = actualException.InnerException;
            }

            // Map exception type to HTTP status code
            var statusCode = actualException switch
            {
                TimeoutException => HttpStatusCode.RequestTimeout,
                DbException => HttpStatusCode.ServiceUnavailable,
                InvalidOperationException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            // Map exception to generic Error object
            var error = actualException switch
            {
                DbException => new UnexpectedError("Database.ConnectionFailure", "Failed to connect to the database. Please try again later."),
                TimeoutException => new UnexpectedError("System.Timeout", "The operation timed out."),
                InvalidOperationException => new UnexpectedError("System.InvalidOperation", "An unexpected operation error occurred."),
                _ => new UnexpectedError("System.Unexpected", "An unexpected error occurred on the server.")
            };

            // Wrap into Result.Failure
            var result = Result.Failure(error);

            var json = JsonSerializer.Serialize(result, _jsonSerializerOptions);

            await context.Response.WriteAsync(json);
        }
    }
}