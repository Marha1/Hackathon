using Domain.Validations;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Enities;

public class User : BaseEntity
{
    public User(string name, bool isAdmin)
    {
        Name = name;
        Admin = isAdmin;
        var validationResult = Validate();
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join("\n", validationResult.Errors);
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
    public ValidationResult Validate()
    {
        var validator = new UserValidation();
        return validator.Validate(this);
    }
}