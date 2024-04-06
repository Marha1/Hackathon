namespace Domain.Primitives;

public class ValidationMessages
{
    public static string IsNullOrEmpty { get; set; } = "Сущность {0} не может быть NULL или пустой!";
    public static string IsValidString { get; set; } = "Значение {0} должно содержать только буквы!";
    public static string IsValidType { get; set; } = "Значение{0} должно иметь корректный тип данных";
    public static string IsMaxAdd { get; set; } = "Пользователем достигнутом максимальное кол-во объявлений";
}