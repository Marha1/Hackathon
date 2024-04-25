using System.Net;
using System.Text.Json;
using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;
using Application.Validations.AdsRequestValidation;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Middleware;

/// <summary>
///     Middleware для Ads
/// </summary>
public class AdsValidationMiddleware
{
    private readonly RequestDelegate _next;

    public AdsValidationMiddleware(RequestDelegate next)
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

                    var validator = new AdsCreateRequestValidation(adsService, userService);
                    var validationResult = await validator.ValidateAsync(request);

                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(string.Join("; ",
                            validationResult.Errors.Select(e => e.ErrorMessage)));
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

                    var request = await JsonSerializer.DeserializeAsync<AdsDeleteRequest>(memoryStream,
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

                    var validator = new AdsDeleteRequestValidation(adsService);
                    var validationResult = await validator.ValidateAsync(request);

                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(string.Join("; ",
                            validationResult.Errors.Select(e => e.ErrorMessage)));
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

                    var validator = new AdsUpdateRequestValidation(adsService);
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