using DiscplinaMobileNoite.Application.UnitOfWork;
using DiscplinaMobileNoite.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DiscplinaMobileNoite.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWorkService _serviceUoW;

        public AuthController(IUnitOfWorkService unitOfWorkService)
        {
            _serviceUoW = unitOfWorkService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordResponse recoverPasswordResponse)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceUoW.RecoverPasswordService.RecoverPassword(recoverPasswordResponse);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}