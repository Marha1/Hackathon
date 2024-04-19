namespace Application.Dtos.AdsDto.Responce;

/// <summary>
///     Дто ответа получения объявления по тексту
/// </summary>
public class AdsGetByTextResponce
{
    /// <summary>
    ///     Id объявления
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Номер объвления
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
    ///     Дата создания объявления
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///     Дата окончания срока объявления
    /// </summary>
    public DateTime ExpirationDate { get; set; }
}