using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Inventory.Application.DTOS;

namespace Inventory.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = exception.Message;
            if (exception.InnerException != null)
            {
                message = $"{exception.Message} - Details: {exception.InnerException.Message}";
                if (exception.InnerException.InnerException != null)
                {
                    message += $" ({exception.InnerException.InnerException.Message})";
                }
            }

            var response = new ApiResponse(false, $"An unexpected error occurred: {message}");
            
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonResponse = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
