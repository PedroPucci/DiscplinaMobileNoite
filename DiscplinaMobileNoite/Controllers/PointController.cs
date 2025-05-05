using DiscplinaMobileNoite.Application.UnitOfWork;
using DiscplinaMobileNoite.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DiscplinaMobileNoite.Controllers
{
    [ApiController]
    [Route("api/v1/points")]
    public class PointController : Controller
    {
        private readonly IUnitOfWorkService _serviceUoW;

        public PointController(IUnitOfWorkService unitOfWorkService)
        {
            _serviceUoW = unitOfWorkService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] PointEntity attendanceRecordEntity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceUoW.AttendanceRecordService.Add(attendanceRecordEntity);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PointEntity>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceUoW.AttendanceRecordService.Get();
            return Ok(result);
        }

        [HttpGet("user/{userId}/date/{date}")]
        public async Task<IActionResult> GetAllByUserAndDate(int userId, DateTime date)
        {
            var registros = await _serviceUoW.AttendanceRecordService.GetByUserIdAndDate(userId, date);
            return Ok(registros);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PointEntity>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var registros = await _serviceUoW.AttendanceRecordService.GetByUserId(userId);

            if (registros == null || !registros.Any())
                return NotFound($"Nenhum ponto encontrado para o usuário {userId}.");

            return Ok(registros);
        }
    }
}