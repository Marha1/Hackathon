using Application.Services.Interfaces;

namespace Application.Middleware;

public class ImageValidationMiddleware
{
    private readonly ILogger<ImageValidationMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ImageValidationMiddleware(RequestDelegate next, ILogger<ImageValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
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
}