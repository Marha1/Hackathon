using Application.Dtos.UserDto.Request;
using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

// Контроллер для работы с пользователями
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IGoogleReCaptchaService _captchaService;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IGoogleReCaptchaService captchaService)
    {
        _userService = userService;
        _captchaService = captchaService;
    }

    // Добавление пользователя
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddUser([FromBody] UserCreateRequest request, [FromQuery] string recaptchaToken)
    {
        var captchaResponse = await _captchaService.VerifyRecaptcha(recaptchaToken);
        if (!captchaResponse.Success) return BadRequest("Ошибка при проверке капчи.");

        if (_userService.Add(request) == null) return BadRequest("Не удалось добавить пользователя.");

        return Ok("Пользователь успешно добавлен.");
    }

    // Получение всех пользователей
    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser()
    {
        var response = await _userService.GetAll();
        if (response == null) return NotFound();
        return Ok(response);
    }

    // Получение пользователя по идентификатору
    [HttpGet("FindById")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById([FromQuery] Guid userId)
    {
        var user = await _userService.FindById(userId);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // Удаление пользователя
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        var deleted = await _userService.Delete(request.Id);
        if (!deleted) return NotFound();
        return Ok("Ok");
    }

    // Обновление пользователя
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put([FromBody] UserUpdateRequest request)
    {
        if (!await _userService.Update(request)) return NotFound();
        return Ok("Ok");
    }
}