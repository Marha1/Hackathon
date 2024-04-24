using System.Net;
using Application.Services.Interfaces;
using Newtonsoft.Json;

namespace Application.Middleware;

/// <summary>
///     Middleware для Images
/// </summary>
public class ImageValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ImageValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        try
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

                if (context.Request.Path.StartsWithSegments("/api/Image/Add") &&
                    context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    // Создаем новый MemoryStream, куда будем записывать данные из оригинального запроса
                    var memoryStream = new MemoryStream();
                    await context.Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Восстанавливаем позицию потока для дальнейшего использования в контроллере
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = memoryStream;

                    var file = context.Request.Form.Files["file"];
                    var idAds = context.Request.Form["idAds"];

                    if (file == null || file.Length == 0 || string.IsNullOrEmpty(idAds))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Данные не переданы или некорректны.");
                        return;
                    }

                    var imageName = await imageService.UploadImages(file, Guid.Parse(idAds));
                    if (imageName is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Не удалось добавить изображение.");
                        return;
                    }
                }
                else if (context.Request.Path.StartsWithSegments("/api/Image/Get") &&
                         context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    var fileName = context.Request.Query["fileName"].ToString();
                    if (string.IsNullOrEmpty(fileName))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Не указано имя файла изображения.");
                        return;
                    }

                    var fileContentResult = await imageService.GetImage(fileName);
                    if (fileContentResult is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("Изображение не найдено.");
                        return;
                    }
                }
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = statusCode,
            ErrorMessage = exception.Message
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}