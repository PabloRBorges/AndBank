using AndBank.Business.Interfaces;
using AndBank.Process.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //■ GET /api/positions/client/{ clientId}/summary: Retorna as últimas posições(agrupe pelo positionId e selecione a
        //última ordenando por data) e some os valores para cada productId.
        //■ GET /api/positions/top10: Retorna as 10 últimas posições com os maiores valores.

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

            if (result == null)
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

            var result = await _positionService.TopClients( 10);

            if (result == null)
                return NotFound(ModelState);

            return Ok(result);
        }
    }
}
