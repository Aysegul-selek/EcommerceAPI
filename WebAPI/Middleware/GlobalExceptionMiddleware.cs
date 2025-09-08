using Application.Dtos.ResponseDto;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponseDto<object>
            {
                Success = false,
                Data = null,
                Message = ex.Message,
                ErrorCodes = ErrorCodes.Exception
            };

            switch (ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ErrorCodes = ErrorCodes.NotFound;
                    break;
                case ConflictException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.ErrorCodes = ErrorCodes.Conflict;
                    break;
                case UnauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.ErrorCodes = ErrorCodes.Unauthorized;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ErrorCodes = ErrorCodes.Exception;
                    break;
            }

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}
