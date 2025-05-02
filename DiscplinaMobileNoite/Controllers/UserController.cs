using DiscplinaMobileNoite.Application.UnitOfWork;
using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DiscplinaMobileNoite.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IUnitOfWorkService _serviceUoW;

        public UserController(IUnitOfWorkService unitOfWorkService)
        {
            _serviceUoW = unitOfWorkService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] UserEntity userEntity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceUoW.UserService.Add(userEntity);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update([FromBody] UserResponse userResponse)
        {
            var result = await _serviceUoW.UserService.Update(userResponse);
            return result.Success ? Ok(result) : BadRequest(userResponse);
        }
    }
}