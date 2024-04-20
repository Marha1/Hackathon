using System.Text.Json;
using Application.Dtos.UserDto.Request;
using Application.Services.Interfaces;

namespace Application.Middleware;

public class UserValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var captchaService = scope.ServiceProvider.GetRequiredService<IGoogleReCaptchaService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            if (context.Request.Path.StartsWithSegments("/api/User/Add") &&
                context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var token = context.Request.Query["recaptchaToken"].ToString();
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

                var request = await JsonSerializer.DeserializeAsync<UserCreateRequest>(memoryStream,
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

                if (request.Name == null || request.Name == string.Empty)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Имя не может быть пустым");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/User/Get") &&
                     context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // Обработка получения всех пользователей
                var response = await userService.GetAll();
                if (response.Users == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Пользователей не найдено.");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/User/FindById") &&
                     context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // Обработка получения пользователя по идентификатору
                var userIdString = context.Request.Query["userId"].ToString();
                if (!Guid.TryParse(userIdString, out var userId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Неверный формат идентификатора пользователя.");
                    return;
                }

                var user = await userService.FindById(userId);
                if (user == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Пользователь не найден.");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/User/Delete") &&
                     context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                // Обработка удаления пользователя
                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var request = await JsonSerializer.DeserializeAsync<UserDeleteRequest>(memoryStream,
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

                var deleted = await userService.Delete(request.Id);
                if (!deleted)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Пользователь не найден.");
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/api/User/Update") &&
                     context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                // Обработка обновления пользователя
                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var request = await JsonSerializer.DeserializeAsync<UserUpdateRequest>(memoryStream,
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
                    await context.Response.WriteAsync("Id пользователя не может быть пустым!");
                    return;
                }

                if (userService.FindById(request.Id) == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Пользователь не найден.");
                    return;
                }
            }
        }

        await _next(context);
    }
}