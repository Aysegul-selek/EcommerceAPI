using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Application.Dtos.ResponseDto;

namespace WebAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(1);

            // Timeout middleware
            app.Use(async (context, next) =>
            {
                var task = next(context);
                var delayTask = Task.Delay(timeout.Value);

                var completedTask = await Task.WhenAny(task, delayTask);

                if (completedTask == delayTask)
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                        var respınse = new ApiResponseDto<object>
                        {
                            Success = false,
                            Message = "İstek zaman aşımına uğradı.",
                            Data = null,
                            ErrorCodes = ErrorCodes.Timeout
                        };
                        context.Response.ContentType = "application/json";
                        var json = System.Text.Json.JsonSerializer.Serialize(respınse);
                        await context.Response.WriteAsync(json);

                    }
                    return;
                }

                await task;
            });

            // Yetkisiz durumları yakalama
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == 401 || response.StatusCode == 403)
                {
                    response.ContentType = "application/json";

                    var result = new ApiResponseDto<object>
                    {
                        Success = false,
                        Message = response.StatusCode == 401
                            ? "Kullanıcı yetkili değil veya token geçersiz."
                            : "Erişim yetkiniz yok.",
                        Data = null,
                        ErrorCodes = ErrorCodes.Unauthorized
                    };

                    var json = System.Text.Json.JsonSerializer.Serialize(result);
                    await response.WriteAsync(json);
                }
            });

            return app;
        }
    }
}
