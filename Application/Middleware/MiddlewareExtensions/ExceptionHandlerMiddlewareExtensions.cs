using Microsoft.AspNetCore.Diagnostics;

namespace Application.Middleware.MiddlewareExtensions;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}