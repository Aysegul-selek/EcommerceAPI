using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Header'da varsa kullan, yoksa yeni oluştur
            if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Response header olarak ekle
            context.Response.Headers[CorrelationHeader] = correlationId;

            // HttpContext.Items içine de koy, başka yerlerde lazım olabilir
            context.Items["CorrelationId"] = correlationId;

            // Serilog log context’e property olarak ekle
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
