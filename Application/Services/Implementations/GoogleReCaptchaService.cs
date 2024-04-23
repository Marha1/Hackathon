using Application.Dtos.GoogleReCaptchaDto;
using Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Services.Implementations;

/// <summary>
///     Реализация сервиса для работы с Google reCAPTCHA.
/// </summary>
public class GoogleReCaptchaService : IGoogleReCaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleReCaptchaSettings> _options;

    public GoogleReCaptchaService(HttpClient httpClient, IOptions<GoogleReCaptchaSettings> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    ///     Проверяет токен Google reCAPTCHA.
    /// </summary>
    /// <param name="recaptchaToken">Токен, полученный от клиента.</param>
    /// <returns>Ответ от Google reCAPTCHA, содержащий результат проверки.</returns>
    public async Task<GoogleReCaptchaResponse> VerifyRecaptcha(string recaptchaToken)
    {
        var request = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", _options.Value.SecretKey),
            new KeyValuePair<string, string>("response", recaptchaToken)
        });

        var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", request);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var captchaResponse = JsonConvert.DeserializeObject<GoogleReCaptchaResponse>(jsonString);
            if (captchaResponse != null) return captchaResponse;
        }

        return new GoogleReCaptchaResponse
            { Success = false, ErrorMessage = "Ошибка при запросе к серверу Google reCAPTCHA." };
    }
}