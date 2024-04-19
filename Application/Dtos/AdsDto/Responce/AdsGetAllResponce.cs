namespace Application.Dtos.AdsDto.Responce;

/// <summary>
///     Дто ответа на получение всех объявлений
/// </summary>
public class AdsGetAllResponce
{
    /// <summary>
    ///     Id объявления
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Номер объявления
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    ///     Текст объявления
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Рейтинг объявления
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///     Дата окончания срока объявления
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    ///     Список ссылок на изображения объвления
    /// </summary>
    public List<string> Images { get; set; }

    /// <summary>
    ///     Id пользователя которму принадлежит объявление
    /// </summary>
    public Guid UserId { get; set; }
}