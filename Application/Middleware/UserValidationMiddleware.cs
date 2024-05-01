using System.Net;
using System.Text.Json;
using Application.Dtos.UserDto.Request;
using Application.Dtos.UserDto.Responce;
using Application.Services.Interfaces;
using Application.Validations.UserRequestValidation;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Middleware;

/// <summary>
///     Middleware для User
/// </summary>
public class UserValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        try
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var captchaService = scope.ServiceProvider.GetRequiredService<IGoogleReCaptchaService>();

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

                    var validator = new UserCreateRequestValidation();
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(string.Join("; ",
                            validationResult.Errors.Select(e => e.ErrorMessage)));
                        return;
                    }
                }
                else if (context.Request.Path.StartsWithSegments("/api/User/Get") &&
                         context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    // // Обработка получения всех пользователей
                    // var response = await userService.GetAll();
                    // if (response is null)
                    // {
                    //     context.Response.StatusCode = StatusCodes.Status404NotFound;
                    //     await context.Response.WriteAsync("Пользователей не найдено.");
                    //     return;
                    // }
                }
                else if (context.Request.Path.StartsWithSegments("/api/User/FindById") &&
                         context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    var userIdString = context.Request.Query["userId"].ToString();
                    if (!Guid.TryParse(userIdString, out var userId))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Неверный формат идентификатора пользователя.");
                        return;
                    }

                    var validator = new UserGetByIdRequestValidation();
                    var validationResult = await validator.ValidateAsync(new UserGetByIdResponse { Id = userId });

                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        foreach (var error in validationResult.Errors)
                            await context.Response.WriteAsync(error.ErrorMessage);
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

                    var validator = new UserDeleteRequestValidation();
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(string.Join("; ",
                            validationResult.Errors.Select(e => e.ErrorMessage)));
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

                    var validator = new UserUpdateRequestValidation();
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(string.Join("; ",
                            validationResult.Errors.Select(e => e.ErrorMessage)));
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