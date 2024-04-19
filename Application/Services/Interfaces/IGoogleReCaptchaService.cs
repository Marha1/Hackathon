using Application.Dtos.GoogleReCaptchaDto;

namespace Application.Services.Interfaces;

/// <summary>
///     Интерфейс сервиса Google reCAPTCHA.
/// </summary>
public interface IGoogleReCaptchaService
{
    /// <summary>
    ///     Проверяет токен Google reCAPTCHA.
    /// </summary>
    /// <param name="recaptchaToken">Токен Google reCAPTCHA для проверки.</param>
    /// <returns>Ответ от Google reCAPTCHA API.</returns>
    Task<GoogleReCaptchaResponse> VerifyRecaptcha(string recaptchaToken);
}