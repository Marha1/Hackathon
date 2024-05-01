using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

/// <summary>
///     Контроллер для работы с изображениями
/// </summary>
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    /// <summary>
    ///     Получение изображения по имени файла
    /// </summary>
    /// <param name="fileName">Имя файла изображения</param>
    /// <returns>Изображение в формате FileContentResult</returns>
    [HttpGet("GetImage")]
    [ProducesResponseType(typeof(FileContentResult),
        StatusCodes.Status200OK)] // Возвращает изображение при успешном запросе
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Возвращает статус "не найдено" при отсутствии изображения
    public async Task<IActionResult> GetImage(string fileName)
    {
        var fileContentResult = await _imageService.GetImage(fileName);
        if (fileContentResult is null) return NotFound();
        return fileContentResult;
    }

    /// <summary>
    ///     Добавление изображения
    /// </summary>
    /// <param name="file">Файл изображения</param>
    /// <param name="idAds">Идентификатор объявления</param>
    /// <returns>Статус 200 OK при успешном добавлении</returns>
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном добавлении изображения
    [ProducesResponseType(StatusCodes
        .Status400BadRequest)] // Возвращает статус "неверный запрос" при некорректном запросе
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> AddImage(IFormFile file, Guid idAds)
    {
        var imageName = await _imageService.UploadImages(file, idAds);
        if (imageName is null) return BadRequest();
        return Ok();
    }

    /// <summary>
    ///     Получение измененного и сохраненного изображения
    /// </summary>
    /// <param name="fileName">Имя файла изображения</param>
    /// <param name="width">Ширина измененного изображения</param>
    /// <param name="height">Высота измененного изображения</param>
    /// <returns>Измененное изображение в формате FileContentResult</returns>
    [HttpGet("GetImageResize")]
    [ProducesResponseType(typeof(FileContentResult),
        StatusCodes.Status200OK)] // Возвращает измененное изображение при успешном запросе
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Возвращает статус "не найдено" при отсутствии изображения
    public async Task<IActionResult> GetImageResize([FromQuery] string fileName, int width, int height)
    {
        var fileContentResult = await _imageService.ResizeAndSaveImage(fileName, width, height);
        if (fileContentResult is null) return NotFound();
        return fileContentResult;
    }
}