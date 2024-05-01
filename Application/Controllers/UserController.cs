using Application.Dtos.UserDto.Request;
using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

/// <summary>
///     Контроллер для работы с пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Добавление пользователя
    /// </summary>
    /// <param name="request">Запрос на создание пользователя</param>
    /// <returns>Результат операции</returns>
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном добавлении пользователя
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Возвращает статус ошибки запроса при некорректных данных
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> AddUser([FromBody] UserCreateRequest request)
    {
        await _userService.Add(request);
        return Ok("Пользователь успешно добавлен.");
    }

    /// <summary>
    ///     Получение всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<User>),
        StatusCodes.Status200OK)] // Возвращает список пользователей при успешном запросе
    [ProducesResponseType(StatusCodes
        .Status404NotFound)] // Возвращает статус ошибки "не найдено" при отсутствии пользователей
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> GetUser()
    {
        var response = await _userService.GetAll();
        return Ok(response);
    }

    /// <summary>
    ///     Получение пользователя по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Пользователь с указанным идентификатором</returns>
    [HttpGet("FindById")]
    [ProducesResponseType(typeof(User),
        StatusCodes.Status200OK)] // Возвращает пользователя с указанным идентификатором при успешном запросе
    [ProducesResponseType(StatusCodes
        .Status404NotFound)] // Возвращает статус ошибки "не найдено" при отсутствии пользователя с указанным идентификатором
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> GetUserById([FromQuery] Guid userId)
    {
        var user = await _userService.FindById(userId);
        return Ok(user);
    }

    /// <summary>
    ///     Удаление пользователя
    /// </summary>
    /// <param name="request">Запрос на удаление пользователя</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном удалении пользователя
    [ProducesResponseType(StatusCodes
        .Status404NotFound)] // Возвращает статус ошибки "не найдено" при отсутствии пользователя для удаления
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        var deleted = await _userService.Delete(request.Id);
        if (!deleted) return NotFound();
        return Ok("Ok");
    }

    /// <summary>
    ///     Обновление пользователя
    /// </summary>
    /// <param name="request">Запрос на обновление пользователя</param>
    /// <returns>Результат операции</returns>
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает успешный статус при успешном обновлении пользователя
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Возвращает статус ошибки запроса при некорректных данных
    [ProducesResponseType(StatusCodes
        .Status500InternalServerError)] // Возвращает статус ошибки сервера при внутренней ошибке
    public async Task<IActionResult> Put([FromBody] UserUpdateRequest request)
    {
        if (!await _userService.Update(request)) return NotFound();
        return Ok("Ok");
    }
}