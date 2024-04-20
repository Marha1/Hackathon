using Domain.Validations;
using FluentValidation;

namespace Domain.Enities;

public class User : BaseEntity
{
    public User(string name, bool isAdmin)
    {
        Name = name;
        Admin = isAdmin;
        var validationErrors = Validate();
        if (validationErrors.Any())
        {
            var errorMessages = string.Join("\n", validationErrors);
            throw new ValidationException(errorMessages);
        }
    }

    public User()
    {
    }

    /// <summary>
    ///     Имя пользователя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Статус администратора у пользователя
    /// </summary>
    public bool Admin { get; set; }

    /// <summary>
    ///     Список объявлений связанных с пользователем
    /// </summary>
    public ICollection<Ads> Ads { get; set; }

    /// <summary>
    ///     Переопределяет метод сравнения объектов для пользовательского класса.
    /// </summary>
    /// <param name="obj">Объект для сравнения.</param>
    /// <returns>True, если объекты равны, в противном случае - false.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is User)) return false;
        var other = (User)obj;
        return Id == other.Id;
    }

    /// <summary>
    ///     Вычисляет хеш-код для пользовательского объекта.
    /// </summary>
    /// <returns>Хеш-код объекта.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    ///     Проверяет валидность пользовательского объекта.
    /// </summary>
    /// <returns>Результат валидации.</returns>
    /// <summary>
    ///     Проверяет валидность пользовательского объекта.
    /// </summary>
    /// <returns>Список сообщений об ошибках валидации.</returns>
    public List<string> Validate()
    {
        var validator = new UserValidation();
        var validationResult = validator.Validate(this);
        return validationResult.Errors.Select(error => error.ErrorMessage).ToList();
    }
}