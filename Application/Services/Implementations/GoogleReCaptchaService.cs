using Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class GoogleReCaptchaService : IGoogleReCaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<GoogleReCaptchaSettings> _options;

    public GoogleReCaptchaService(HttpClient httpClient, IOptions<GoogleReCaptchaSettings> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

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
            return captchaResponse;
        }
        else
        {
            // Обработка ошибки при запросе к серверу Google reCAPTCHA
            return new GoogleReCaptchaResponse { Success = false, ErrorMessage = "Ошибка при запросе к серверу Google reCAPTCHA." };
        }
    }
}

public class GoogleReCaptchaSettings
{
    public string SiteKey { get; set; }
    public string SecretKey { get; set; }
}

public class GoogleReCaptchaResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}