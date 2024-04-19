namespace Application.Dtos.GoogleReCaptchaDto;

/// <summary>
///     Класс ответа GoogleReCaptcha
/// </summary>
public class GoogleReCaptchaResponse
{
    /// <summary>
    ///     Состояние проверки капчи
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    ///     Сообщениее об ошибке
    /// </summary>
    public string ErrorMessage { get; set; }
}