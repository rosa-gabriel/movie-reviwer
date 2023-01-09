using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly ILogger<ExceptionMiddleware> _logger;
        public readonly RequestDelegate _next;
        public readonly IHostEnvironment _enviroment;
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next, IHostEnvironment environment)
        {
            this._logger = logger;
            this._next = next;
            this._enviroment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _enviroment.IsDevelopment() ? new ExceptionResponse(context.Response.StatusCode, ex.Message, ex.StackTrace) : new ExceptionResponse(context.Response.StatusCode, ex.Message);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}