using System.Text.Json;
using FluentValidation;
using ProductChallenge.Application.Common.Exceptions;

namespace ProductChallenge.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
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

            var (statusCode, title, errors) = exception switch
            {
                ValidationException validationException => (
                    StatusCodes.Status400BadRequest,
                    "Error de validación",
                    validationException.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })),

                NotFoundException => (
                    StatusCodes.Status404NotFound,
                    "No encontrado.",
                    null),

                ConflictException => (
                    StatusCodes.Status409Conflict,
                    "Conflicto",
                    null),

                ExternalServiceException => (
                    StatusCodes.Status503ServiceUnavailable,
                    "Servicio externo no disponible",
                    null),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Error interno del servidor",
                    null)
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                title,
                status = statusCode,
                detail = exception.Message,
                traceId = context.TraceIdentifier,
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
