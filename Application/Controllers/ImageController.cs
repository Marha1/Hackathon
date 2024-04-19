using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

// Контроллер для работы с изображениями
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    // Получение изображения по имени файла
    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImage(string fileName)
    {
        var fileContentResult = await _imageService.GetImage(fileName);
        if (fileContentResult == null) return NotFound();
        return fileContentResult;
    }

    // Добавление изображения
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddImage(IFormFile file, Guid idAds)
    {
        var imageName = await _imageService.UploadImages(file, idAds);
        if (imageName is null) return BadRequest();
        return Ok();
    }

    // Получение измененного и сохраненного изображения
    [HttpGet("GetImageResize")]
    public async Task<IActionResult> GetImageResize([FromQuery] string fileName, int width, int height)
    {
        var fileContentResult = await _imageService.ResizeAndSaveImage(fileName, width, height);
        if (fileContentResult == null) return NotFound();
        return fileContentResult;
    }
}