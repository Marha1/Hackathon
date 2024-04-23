using Application.Services.Interfaces;
using Infrastructure.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Services.Implementations;

/// <summary>
///     Реализация cервиса для работы с изображениями.
/// </summary>
public class ImageService : IImageService
{
    private readonly IAdsRepository _adsRepository;

    public ImageService(IAdsRepository adsRepository)
    {
        _adsRepository = adsRepository;
    }

    /// <summary>
    ///     Сохраняет изображение.
    /// </summary>
    /// <param name="file">Файл изображения.</param>
    /// <returns>Путь к сохраненному изображению.</returns>
    public async Task<string> SaveImages(IFormFile file)
    {
        if (file is null) return null;

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var pathFolder = Path.Combine("wwwroot", "images");
        Directory.CreateDirectory(pathFolder);

        var filePath = Path.Combine(pathFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("images", fileName);
    }

    /// <summary>
    ///     Загружает изображение и связывает его с объявлением.
    /// </summary>
    /// <param name="file">Файл изображения.</param>
    /// <param name="adsId">Id объявления.</param>
    /// <returns>Имя загруженного изображения.</returns>
    public async Task<string> UploadImages(IFormFile file, Guid adsId)
    {
        var imageName = await SaveImages(file);
        var ads = await _adsRepository.GetById(adsId);
        if (ads is null) return null;
        if (ads.Images is null) ads.Images = new List<string>();
        ads.Images.Add(imageName);
        await _adsRepository.Update(ads);
        return imageName;
    }

    /// <summary>
    ///     Возвращает изображение по имени файла.
    /// </summary>
    /// <param name="fileName">Имя файла изображения.</param>
    /// <returns>Результат выполнения операции.</returns>
    public Task<FileContentResult> GetImage(string fileName)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        var imagePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(imagePath)) return Task.FromResult<FileContentResult>(null);
        var imageBytes = File.ReadAllBytes(imagePath);
        var mimeType = "image/jpeg";
        return Task.FromResult(new FileContentResult(imageBytes, mimeType));
    }

    /// <summary>
    ///     Изменяет размер изображения и сохраняет его.
    /// </summary>
    /// <param name="fileName">Имя файла изображения.</param>
    /// <param name="width">Ширина измененного изображения.</param>
    /// <param name="height">Высота измененного изображения.</param>
    /// <returns>Результат выполнения операции.</returns>
    public async Task<FileContentResult> ResizeAndSaveImage(string fileName, int width, int height)
    {
        var originalImageResult = await GetImage(fileName);
        if (originalImageResult is null) return null;
        using (var stream = new MemoryStream(originalImageResult.FileContents))
        using (var image = Image.Load(stream))
        {
            image.Mutate(x => x.Resize(width, height));

            byte[] resizedImageBytes;
            using (var outputStream = new MemoryStream())
            {
                await image.SaveAsJpegAsync(outputStream);
                resizedImageBytes = outputStream.ToArray();
            }

            var mimeType = "image/jpeg";
            var pathFolder = Path.Combine("wwwroot", "images", "resize");
            Directory.CreateDirectory(pathFolder);
            var resizedImagePath = Path.Combine(pathFolder, fileName);
            using (var outputStream = new FileStream(resizedImagePath, FileMode.Create))
            {
                await image.SaveAsJpegAsync(outputStream);
            }

            return new FileContentResult(resizedImageBytes, mimeType);
        }
    }
}