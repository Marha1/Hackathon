using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces;

/// <summary>
///     Интерфейс сервиса для работы с изображениями.
/// </summary>
public interface IImageService
{
    /// <summary>
    ///     Сохраняет изображение.
    /// </summary>
    /// <param name="file">Файл изображения для сохранения.</param>
    /// <returns>Путь к сохраненному изображению.</returns>
    public Task<string> SaveImages(IFormFile file);

    /// <summary>
    ///     Загружает изображение и связывает его с объявлением.
    /// </summary>
    /// <param name="file">Файл изображения для загрузки.</param>
    /// <param name="adsId">Id объявления, с которым связывается изображение.</param>
    /// <returns>Путь к загруженному изображению.</returns>
    public Task<string> UploadImages(IFormFile file, Guid adsId);

    /// <summary>
    ///     Получает изображение по его имени файла.
    /// </summary>
    /// <param name="fileName">Имя файла изображения.</param>
    /// <returns>Результат содержащий содержимое файла изображения.</returns>
    public Task<FileContentResult> GetImage(string fileName);

    /// <summary>
    ///     Изменяет размер изображения и сохраняет его.
    /// </summary>
    /// <param name="fileName">Имя файла изображения.</param>
    /// <param name="width">Ширина измененного изображения.</param>
    /// <param name="height">Высота измененного изображения.</param>
    /// <returns>Результат содержащий содержимое измененного файла изображения.</returns>
    public Task<FileContentResult> ResizeAndSaveImage(string fileName, int width, int height);
}