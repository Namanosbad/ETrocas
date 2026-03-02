using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ETrocas.API.Internal.Middleware;

/// <summary>
/// Middleware global para tratamento padronizado de exceções da API.
/// </summary>
public static class ApiExceptionMiddlewareExtensions
{
    /// <summary>
    /// Registra o middleware de tratamento global de exceções.
    /// </summary>
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiExceptionMiddleware>();
    }

    private sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Erro não tratado durante o processamento da requisição.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Não autorizado"),
                InvalidOperationException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
            };

            var details = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            details.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(details));
        }
    }
}
