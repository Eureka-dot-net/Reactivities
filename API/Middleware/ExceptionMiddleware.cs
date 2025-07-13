using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Application.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Unauthorized access."
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = env.IsDevelopment()
                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new AppException(context.Response.StatusCode, "An unexpected error occurred.", null);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };


            await context.Response.WriteAsJsonAsync(response, options);
        }

        private static async Task HandleValidationExceptionAsync(HttpContext context, FluentValidation.ValidationException ex)
        {
            var validationErrors = new Dictionary<string, string[]>();
            if (ex.Errors is not null)
            {
                foreach (var error in ex.Errors)
                {
                    if (!validationErrors.ContainsKey(error.PropertyName))
                    {
                        // use new .net 9 collection initializer syntax
                        validationErrors[error.PropertyName] = [error.ErrorMessage];
                    }
                    else
                    {
                        var existingErrors = validationErrors[error.PropertyName].ToList();
                        existingErrors.Add(error.ErrorMessage);
                        validationErrors[error.PropertyName] = existingErrors.ToArray();
                    }
                }
            }
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationProblemDetails = new ValidationProblemDetails(validationErrors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors occurred."
            };
            await context.Response.WriteAsJsonAsync(validationProblemDetails);
        }
    }
}
