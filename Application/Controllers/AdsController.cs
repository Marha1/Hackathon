using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;
using Domain.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly IAdsService _adsService;
        private readonly IGoogleReCaptchaService _captchaService;

        public AdsController(IAdsService adsService, IGoogleReCaptchaService captchaService)
        {
            _adsService = adsService;
            _captchaService = captchaService;
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAds([FromBody] AdsCreateRequest request, [FromQuery] string token)
        {
            var captchaResponse = await _captchaService.VerifyRecaptcha(token);
            if (!captchaResponse.Success)
            {
                return BadRequest("Ошибка при проверке капчи.");
            }

            if (!_adsService.TryToPublic(request.UserId))
            {
                return BadRequest("Пользователь достиг максимального количества объявлений");
            }

            if (_adsService.Add(request) == null)
            {
                return BadRequest("Пользователь не найден");
            }
            
            return Ok();
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAds()
        {
            var responce = await _adsService.GetAll();
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
        public async Task<IActionResult> Delete([FromBody] AdsDeleteRequest request)
        {
            var deleted = await _adsService.Delete(request.Id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Ok");
        }

        [HttpGet("DescendingFiltration")]
        [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DescendingFiltration()
        {
            var responce = await _adsService.Filtration();
            if (responce == null)
            {
                return NotFound();
            }

            return Ok(responce);
        }

        [HttpGet("AscendingFiltration")]
        [ProducesResponseType(typeof(IEnumerable<Ads>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AscendingFiltration()
        {
            var responce = await _adsService.AscendingFiltration();
            if (responce == null)
            {
                return NotFound();
            }

            return Ok(responce);
        }

        [HttpGet("FindByText")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByText([FromQuery] string adsText)
        {
            var ads = await _adsService.FindByText(adsText);
            if (ads == null)
            {
                return NotFound();
            }

            return Ok(ads);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromBody] AdsUpdateRequest request)
        {
            if (!await _adsService.Update(request))
            {
                return NotFound();
            }

            return Ok("Ok");
        }
    }
}
