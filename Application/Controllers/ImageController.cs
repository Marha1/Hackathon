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
    public IActionResult GetImage(string fileName)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            
        string imagePath = Path.Combine(folderPath, fileName);
            
        if (!System.IO.File.Exists(imagePath))
        {
            return NotFound();
        }
            
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            
        string mimeType = "image/jpeg";

        return File(imageBytes, mimeType);
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

}