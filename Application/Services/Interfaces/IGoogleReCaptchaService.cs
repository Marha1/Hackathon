using Application.Dtos.GoogleReCaptchaDto;

namespace Application.Services.Interfaces;

public interface IGoogleReCaptchaService
{
    Task<GoogleReCaptchaResponse> VerifyRecaptcha(string recaptchaToken);

}