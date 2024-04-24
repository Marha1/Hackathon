namespace Domain.Primitives;

/// <summary>
///     Класс списка ошибок
/// </summary>
public class ValidationMessages
{
    /// <summary>
    ///     Ошибка о Null/пустом объекте
    /// </summary>
    public static string IsNullOrEmpty { get; set; } = "Сущность {0} не может быть NULL или пустой!";

    /// <summary>
    ///     Ошибка наличия каких-либо символов кроме букв
    /// </summary>
    public static string IsValidString { get; set; } = "Значение {0} должно содержать только буквы!";

    /// <summary>
    ///     Ошибка типа данных
    /// </summary>
    public static string IsValidType { get; set; } = "Значение{0} должно иметь корректный тип данных";

    /// <summary>
    ///     Ошибка максимального значениня
    /// </summary>
    public static string IsMaxLength { get; set; } = "Значение{0} не может быть длиннее 100 символов";

    /// <summary>
    ///     Ошибка  о максимаьном кол-ве объявлений
    /// </summary>
    public static string IsMaxPublic { get; set; } = "Достигнуто максимальное кол-во публикаций";

    /// <summary>
    ///     ошибка не найденной сущности
    /// </summary>
    public static string NotFound { get; set; } = "{0} не найден(о)";
}