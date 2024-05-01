using System.Net;
using System.Text.Json;
using Application.Dtos.AdsDto.Request;
using Application.Dtos.UserDto.Request;
using Application.Dtos.UserDto.Responce;
using Application.Services.Interfaces;
using Application.Validations.AdsRequestValidation;
using Application.Validations.UserRequestValidation;
using FluentValidation;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public ValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (context.Request.Path.StartsWithSegments("/api/User"))
                await ValidateUserRequests(context, _serviceProvider);
            else if (context.Request.Path.StartsWithSegments("/api/Ads"))
                await ValidateAdsRequests(context, _serviceProvider);
            else if (context.Request.Path.StartsWithSegments("/api/Image")) await ValidateImageRequests(context);
            if (context.Response.StatusCode != StatusCodes.Status400BadRequest) await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task ValidateUserRequests(HttpContext context, IServiceProvider serviceProvider)
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

                context.Request.EnableBuffering();
                var request = await DeserializeRequestBodyAsync<UserCreateRequest>(context.Request);
                context.Request.Body.Position = 0;
                await ValidateAndHandleResultAsync(new UserCreateRequestValidation(), request, context);
            }

            if (context.Request.Path.StartsWithSegments("/api/User/Delete") &&
                context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();
                var request = await DeserializeRequestBodyAsync<UserDeleteRequest>(context.Request);
                context.Request.Body.Position = 0;
                await ValidateAndHandleResultAsync(new UserDeleteRequestValidation(), request, context);

                if (request is null)
                {
                    context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                    await context.Response.WriteAsync("Формат тела запроса неподдерживаемый.");
                }
            }

            if (context.Request.Path.StartsWithSegments("/api/User/Update") &&
                context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();
                var request = await DeserializeRequestBodyAsync<UserUpdateRequest>(context.Request);
                context.Request.Body.Position = 0;
                await ValidateAndHandleResultAsync(new UserUpdateRequestValidation(), request, context);
                if (request is null)
                {
                    context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                    await context.Response.WriteAsync("Формат тела запроса неподдерживаемый.");
                }
            }

            if (context.Request.Path.StartsWithSegments("/api/User/FindById") &&
                context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();
                var request = await DeserializeRequestBodyAsync<UserGetByIdResponse>(context.Request);
                context.Request.Body.Position = 0;
                await ValidateAndHandleResultAsync(new UserGetByIdRequestValidation(), request, context);
            }
        }
    }

    private async Task ValidateAdsRequests(HttpContext context, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var captchaService = scope.ServiceProvider.GetRequiredService<IGoogleReCaptchaService>();
            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var token = context.Request.Query["token"].ToString();
                var captchaResponse = await captchaService.VerifyRecaptcha(token);
                if (!captchaResponse.Success)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Ошибка при проверке капчи.");
                }
                context.Request.EnableBuffering();
                var request = await DeserializeRequestBodyAsync<AdsCreateRequest>(context.Request);
                context.Request.Body.Position = 0;
                await ValidateAndHandleResultAsync(new AdsCreateRequestValidation(), request, context);
            }
        }

        if (context.Request.Path.StartsWithSegments("/api/Ads/Filtration") &&
            context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            var adsText = context.Request.Query["text"].ToString();

            if (string.IsNullOrWhiteSpace(adsText))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Текст объявления не может быть пустым.");
                return;
            }
        }

        if (context.Request.Path.StartsWithSegments("/api/Ads/Delete") &&
            context.Request.Method.Equals("Delete", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.EnableBuffering();
            var request = await DeserializeRequestBodyAsync<AdsDeleteRequest>(context.Request);
            context.Request.Body.Position = 0;
            await ValidateAndHandleResultAsync(new AdsDeleteRequestValidation(), request, context);
        }

        if (context.Request.Path.StartsWithSegments("/api/Ads/Update") &&
            context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.EnableBuffering();
            var request = await DeserializeRequestBodyAsync<AdsUpdateRequest>(context.Request);
            context.Request.Body.Position = 0;
            await ValidateAndHandleResultAsync(new AdsUpdateRequestValidation(), request, context);
        }
    }

    private async Task ValidateImageRequests(HttpContext context)
    {
        if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            var file = context.Request.Form.Files["file"];
            var idAds = context.Request.Form["idAds"];

            if (file == null || file.Length == 0 || string.IsNullOrEmpty(idAds))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Данные не переданы или некорректны.");
            }
        }

        if (context.Request.Path.StartsWithSegments("/api/Image/Get") &&
            context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            var fileName = context.Request.Query["fileName"].ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Не указано имя файла изображения.");
            }
        }
    }

    private async Task<T> DeserializeRequestBodyAsync<T>(HttpRequest request)
    {
        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(requestBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }


    private async Task ValidateAndHandleResultAsync<T>(AbstractValidator<T> validator, T request, HttpContext context)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = statusCode,
            ErrorMessage = exception.Message
        });
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(result);
    }
}