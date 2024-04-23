using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

// Контроллер для управления объявлениями
[Route("api/[controller]")]
public class AdsController : ControllerBase
{
    private readonly IAdsService _adsService;

    public AdsController(IAdsService adsService)
    {
        _adsService = adsService;
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAds([FromBody] AdsCreateRequest request, [FromQuery] string recaptchaToken)
    {
        await _adsService.Add(request);
        return Ok();
    }

    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAds()
    {
        return Ok(await _adsService.GetAll());
    }

    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromBody] AdsDeleteRequest request)
    {
        await _adsService.Delete(request.Id);
        return Ok("Успешно удалено");
    }

    [HttpGet("Filtration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Filtration(string text, [FromQuery] bool ascending = false)
    {
        var responce = await _adsService.Filtration(ascending,text);
        return Ok(responce);
    }
    
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put([FromBody] AdsUpdateRequest request)
    {
        var ads = await _adsService.Update(request);
        return Ok(ads);
    }
}