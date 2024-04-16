using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Application.Controllers;

[Route("api/[controller]")]
public class ImageController: ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }
    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImage(string fileName)
    {
        var fileContentResult = await _imageService.GetImage(fileName);
        if (fileContentResult == null)
        {
            return NotFound(); 
        }
        return fileContentResult;
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddImage(IFormFile file,Guid idAds)
    {
       
        var imageName = _imageService.UploadImages(file, idAds);
        if (imageName is null)
        {
            return BadRequest();
        }

        return Ok();

    }
    [HttpGet("GetImageResize")]
    public async Task<IActionResult> GetImageResize([FromQuery]string fileName,int widht,int hight)
    {
        var fileContentResult = await _imageService.ResizeAndSaveImage(fileName,widht,hight);
        if (fileContentResult == null)
        {
            return NotFound(); 
        }
        return fileContentResult;
    }

}