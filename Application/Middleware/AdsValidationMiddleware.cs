using System.Text.Json;
using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;

namespace Application.Middleware;

public class AdsValidationMiddleware
{
    private readonly RequestDelegate _next;

    public AdsValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var captchaService = scope.ServiceProvider.GetRequiredService<IGoogleReCaptchaService>();
            var adsService = scope.ServiceProvider.GetRequiredService<IAdsService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            if (context.Request.Path.StartsWithSegments("/api/Ads/FindByText") &&
                context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var adsText = context.Request.Query["adsText"].ToString();

                if (string.IsNullOrWhiteSpace(adsText))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Текст объявления не может быть пустым.");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/Ads/Add") &&
                     context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var token = context.Request.Query["token"].ToString();
                var captchaResponse = await captchaService.VerifyRecaptcha(token);
                if (!captchaResponse.Success)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Ошибка при проверке капчи.");
                    return;
                }

                // Создаем новый MemoryStream, куда будем записывать данные из оригинального запроса
                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var request = await JsonSerializer.DeserializeAsync<AdsCreateRequest>(memoryStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // Восстанавливаем позицию потока для дальнейшего использования в контроллере
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Заменяем тело запроса на наш MemoryStream
                context.Request.Body = memoryStream;

                if (request == null)
                {
                    Console.WriteLine("Request is null");
                    context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                    await context.Response.WriteAsync("Формат тела запроса неподдерживаемый.");
                    return;
                }


                if (request.UserId == Guid.Empty)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Id пользователя не может быть пустым!");
                    return;
                }

                if (await userService.FindById(request.UserId) is null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Пользователь не найден");
                    return;
                }

                if (!await adsService.TryToPublic(request.UserId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Пользователь достиг максимального количества объявлений");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/Ads/Get") &&
                     context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                if (await adsService.GetAll() is null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Объявлений не найдено");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/Ads/Delete") &&
                     context.Request.Method.Equals("Delete", StringComparison.OrdinalIgnoreCase))
            {
                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var request = await JsonSerializer.DeserializeAsync<AdsCreateRequest>(memoryStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // Восстанавливаем позицию потока для дальнейшего использования в контроллере
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Заменяем тело запроса на наш MemoryStream
                context.Request.Body = memoryStream;
                if (request == null)
                {
                    context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                    await context.Response.WriteAsync("Формат тела запроса неподдерживаемый.");
                    return;
                }

                if (request.Id == Guid.Empty)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Id не может быть пустым");
                    return;
                }


                if (!await adsService.Delete(request.Id))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Объявление не найдено");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/Ads/Update") &&
                     context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var request = await JsonSerializer.DeserializeAsync<AdsUpdateRequest>(memoryStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // Восстанавливаем позицию потока для дальнейшего использования в контроллере
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Заменяем тело запроса на наш MemoryStream
                context.Request.Body = memoryStream;

                if (request == null)
                {
                    context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                    await context.Response.WriteAsync("Формат тела запроса неподдерживаемый.");
                    return;
                }

                if (request.Id == Guid.Empty)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Id объявлением не может быть пустым");
                    return;
                }

                // Проверка на обновление объявления
                if (!await adsService.Update(request))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Объявление не найдено");
                    return;
                }
            }
        }

        await _next(context);
    }
}