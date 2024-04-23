using Domain.Validations;
using ValidationException = FluentValidation.ValidationException;

namespace Domain.Enities;

/// <summary>
///     Доменная сущность объявления
/// </summary>
public class Ads : BaseEntity
{
    public Ads(int number, string text, int rating, DateTime created, DateTime expirationDate, Guid userId)
    {
        Number = number;
        Text = text;
        Rating = rating;
        Created = created;
        ExpirationDate = expirationDate;
        UserId = userId;
        Images = new List<string>();

        var validationErrors = Validate();
        if (validationErrors.Any())
        {
            var errorMessages = string.Join("\n", validationErrors);
            throw new ValidationException(errorMessages);
        }
    }

    public Ads()
    {
    }

    /// <summary>
    ///     Номер объявления.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    ///     Текст объявления.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Рейтинг объявления.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    ///     Дата создания объявления.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///     Дата истечения срока действия объявления.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    ///     Список изображений, связанных с объявлением.
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    ///     Id пользователя, связанного с объявлением.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Cвязанный с объявлением пользователь.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    ///     Переопределение метода Equals для сравнения объектов типа Ads.
    /// </summary>
    /// <param name="obj">Объект для сравнения.</param>
    /// <returns>True, если объекты равны, иначе False.</returns>
    public override bool Equals(object? obj)
    {
        // Проверка на null и тип объекта
        if (obj == null || !(obj is Ads)) return false;
        // Приведение объекта к типу Ads
        var other = (Ads)obj;
        // Сравнение идентификаторов объектов
        return Id == other.Id;
    }

    /// <summary>
    ///     Переопределение метода GetHashCode для вычисления хеш-кода объекта.
    /// </summary>
    /// <returns>Хеш-код объекта.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    ///     Проверяет валидность объявления с помощью валидатора AdsValidation.
    /// </summary>
    /// <returns>Результат валидации.</returns>
    /// <summary>
    ///     Проверяет валидность объявления.
    /// </summary>
    /// <returns>Список сообщений об ошибках валидации.</returns>
    public List<string> Validate()
    {
        var validator = new AdsValidation();
        var validationResult = validator.Validate(this);
        return validationResult.Errors.Select(error => error.ErrorMessage).ToList();
    }
}