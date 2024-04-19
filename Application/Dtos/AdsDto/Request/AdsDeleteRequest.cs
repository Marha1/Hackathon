namespace Application.Dtos.AdsDto.Request;

/// <summary>
///     Дто запроса на удаление объявления
/// </summary>
public class AdsDeleteRequest
{
    /// <summary>
    ///     Id объявления для удаления
    /// </summary>
    public Guid Id { get; set; }
}