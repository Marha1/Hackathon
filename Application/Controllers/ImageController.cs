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