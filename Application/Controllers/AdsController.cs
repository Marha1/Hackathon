using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;
[Route("api/[controller]")]

public class AdsController: ControllerBase
{
    private readonly IAdsService _adsService;

        public AdsController(IAdsService adsService)
        {
            this._adsService = adsService;
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult AddAds([FromBody] Ads request)
        {
            if (!_adsService.TryToPublic(request.UserId))
            {
                return BadRequest("Пользователь достиг максимальное кол-во объявлений");
            }

            if (_adsService.Add(request)==null)
            {
                return BadRequest("Пользователь не найден");
            }
            return Ok();
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAds()
        {
            var responce = _adsService.GetAll();
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Delete([FromBody] Ads request)
        {
            var deleted = _adsService.Delete(request.Id);
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

        public IActionResult Put([FromBody] Ads request)
        {
            if (!_adsService.Update(request))
            { 
                return NotFound();
            }
            return Ok("Ok");
        }
    
}