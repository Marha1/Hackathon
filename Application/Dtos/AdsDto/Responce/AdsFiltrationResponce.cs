namespace Application.Dtos.AdsDto.Responce;

// Ответ на запрос фильтрации объявлений
public class AdsFiltrationResponce
{
    // Id объявления
    public Guid Id { get; set; }

    // Номер объявления
    public int Number { get; set; }

    // Текст объявления
    public string Text { get; set; }

    // Рейтинг объявления
    public int Rating { get; set; }

    // Дата создания объявления
    public DateTime Created { get; set; }

    // Дата истечения срока действия объявления
    public DateTime ExpirationDate { get; set; }
}