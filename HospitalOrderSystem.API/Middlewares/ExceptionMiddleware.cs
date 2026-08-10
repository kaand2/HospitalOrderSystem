using System.Net;
using System.Text.Json;

namespace HospitalOrderSystem.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                KeyNotFoundException => new
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = exception.Message
                },
                InvalidOperationException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    message = exception.Message
                },
                ArgumentException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    message = exception.Message
                },
                _ => new
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"[{exception.GetType().Name}] {exception.Message}"
                }
            };

            context.Response.StatusCode = response.statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
