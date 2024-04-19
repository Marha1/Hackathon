namespace Application.Dtos.GoogleReCaptchaDto;

/// <summary>
///     Класс настройки GoogleReCaptcha
/// </summary>
public class GoogleReCaptchaSettings
{
    /// <summary>
    ///     Ключ сайта
    /// </summary>
    public string SiteKey { get; set; }

    /// <summary>
    ///     Секретный ключ
    /// </summary>
    public string SecretKey { get; set; }
}