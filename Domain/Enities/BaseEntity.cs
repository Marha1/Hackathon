namespace Domain.Enities;

/// <summary>
///     Базовый абстрактный класс сущности.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    ///     Id сущности.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Переопределение метода для сравнения с другим объектом.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is not BaseEntity entity)
            return false;
        if (entity.Id != Id)
            return false;

        return true;
    }

    /// <summary>
    ///     Переопределение метода для получения хэш-кода объекта.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}