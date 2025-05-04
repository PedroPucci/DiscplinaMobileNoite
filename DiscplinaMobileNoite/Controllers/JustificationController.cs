using DiscplinaMobileNoite.Application.UnitOfWork;
using DiscplinaMobileNoite.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DiscplinaMobileNoite.Controllers
{
    [ApiController]
    [Route("api/v1/justifications")]
    public class JustificationController : Controller
    {
        private readonly IUnitOfWorkService _serviceUoW;

        public JustificationController(IUnitOfWorkService unitOfWorkService)
        {
            _serviceUoW = unitOfWorkService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] JustificationEntity attendanceJustificationEntity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceUoW.AttendanceJustificationService.Add(attendanceJustificationEntity);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}