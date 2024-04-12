﻿using System;
using System.Collections.Generic;
using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application1.Controllers
    {
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;
            private readonly IGoogleReCaptchaService _captchaService;
            public UserController(IUserService userService,IGoogleReCaptchaService captchaService)
            {
                this._userService = userService;
                _captchaService = captchaService;
            }
            [HttpPost("Add")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<IActionResult> AddUser([FromBody] User request, [FromQuery] string recaptchaToken)
            {
                var captchaResponse = await _captchaService.VerifyRecaptcha(recaptchaToken);
                if (!captchaResponse.Success)
                {
                    return BadRequest("Ошибка при проверке капчи.");
                }

                if (_userService.Add(request) == null)
                {
                    return BadRequest("Не удалось добавить пользователя.");
                }

                return Ok("Пользователь успешно добавлен.");
            }

            [HttpGet("Get")]
            [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetUser()
            {
                var responce = _userService.GetAll();
                if (responce == null)
                {
                    return NotFound();
                }
                return Ok(responce);
            }
            [HttpGet("FindById")]
            [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetUserById([FromQuery] Guid userId) 
            {
                var user = _userService.FindById(userId); 
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }

            
            [HttpDelete("Delete")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]

            public IActionResult DeleteUser([FromBody] User request)
            {
                var deleted = _userService.Delete(request.Id);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok("Ok");
            }

            [HttpPut("Update")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]

            public IActionResult Put([FromBody] User request)
            {
                if (!_userService.Update(request))
                { 
                    return NotFound();
                }
                return Ok("Ok");
            }
        }
    }