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

    /// <summary>
    ///     Добавление
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAds([FromBody] AdsCreateRequest request)
    {
        await _adsService.Add(request);
        return Ok();
    }


    // Получение всех объявлений
    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAds()
    {
        return Ok(await _adsService.GetAll());
    }

    // Удаление объявления
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromBody] AdsDeleteRequest request)
    {
        var deleted = await _adsService.Delete(request.Id);
        if (!deleted) return NotFound();

        return Ok("Ok");
    }

    // Фильтрация объявлений
    [HttpGet("Filtration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Filtration([FromQuery] bool ascending = false)
    {
        var responce = await _adsService.Filtration(ascending);
        if (responce is null) return BadRequest();

        return Ok(responce);
    }

    // Поиск объявлений по тексту
    [HttpGet("FindByText")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByText([FromQuery] string adsText)
    {
        var ads = await _adsService.FindByText(adsText);
        if (ads == null) return NotFound();

        return Ok(ads);
    }

    // Обновление объявления
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put([FromBody] AdsUpdateRequest request)
    {
        if (!await _adsService.Update(request)) return NotFound();
        return Ok("Ok");
    }
}