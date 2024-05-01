using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;
using Domain.Enities;
using Domain.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

/// <summary>
///     Контроллер для управления объявлениями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AdsController : ControllerBase
{
    private readonly IAdsService _adsService;

    public AdsController(IAdsService adsService)
    {
        _adsService = adsService;
    }

    /// <summary>
    ///     Добавление нового объявления
    /// </summary>
    /// <param name="request">Запрос на создание объявления</param>
    /// <returns>Результат операции</returns>
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном добавлении объявления
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> AddAds([FromBody] AdsCreateRequest request)
    {
        await _adsService.Add(request);
        return Ok();
    }

    /// <summary>
    ///     Получение всех объявлений
    /// </summary>
    /// <returns>Список всех объявлений</returns>
    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<Ads>),
        StatusCodes.Status200OK)] // Возвращает список объявлений при успешном запросе
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> GetAds()
    {
        return Ok(await _adsService.GetAll());
    }

    /// <summary>
    ///     Удаление объявления
    /// </summary>
    /// <param name="request">Запрос на удаление объявления</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном удалении объявления
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> Delete([FromBody] AdsDeleteRequest request)
    {
        await _adsService.Delete(request.Id);
        return Ok("Успешно удалено");
    }

    /// <summary>
    ///     Фильтрация объявлений
    /// </summary>
    /// <param name="text">Текст для фильтрации</param>
    /// <param name="sortBy">Свойство для сортировки</param>
    /// <param name="ascending">Порядок сортировки</param>
    /// <returns>Отфильтрованный список объявлений</returns>
    [HttpGet("Filtration")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает отфильтрованный список при успешном запросе
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> Filtration(string text, SortBy sortBy, [FromQuery] bool ascending = false)
    {
        var response = await _adsService.Filtration(ascending, sortBy, text);
        if (response is null) return NotFound();
        return Ok(response);
    }

    /// <summary>
    ///     Обновление объявления
    /// </summary>
    /// <param name="request">Запрос на обновление объявления</param>
    /// <returns>Обновленное объявление</returns>
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном обновлении объявления
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> Put([FromBody] AdsUpdateRequest request)
    {
        var ads = await _adsService.Update(request);
        return Ok(ads);
    }
}