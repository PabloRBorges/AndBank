using AndBank.Business.Interfaces;
using AndBank.Process.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ApiFuncional.Controllers
{
    [ApiController]
    [Route("api/positions")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        [Route("clienteId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<PositionViewModel>>> Get(string clienteId)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _positionService.GetPositionsClientById(clienteId);

            if (result == null)
                return NotFound(ModelState);

            return Ok(result);
        }

        [HttpGet]
        [Route("{clienteId}/summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SummaryViewModel>> GetSummary(string clienteId)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _positionService.GetClientSummary(clienteId);

            if (!result.Any())
                return NotFound(ModelState);

            return Ok(result);
        }

        [HttpGet]
        [Route("top10")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PositionViewModel>> GetTop10()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _positionService.TopClients(10);

            if (result == null)
                return NotFound(ModelState);

            return Ok(result);
        }
    }
}
