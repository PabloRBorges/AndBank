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

        [HttpGet("{clientId}/summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SummaryViewModel>> GetProduto(string clienteId)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _positionService.GetClientSummary(clienteId);

            if (result == null)
                return NotFound(ModelState);

            return Ok(result);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult<PositionViewModel>> PostProduto(PositionViewModel produto)
        //{
        //    if (_positionRepository.Produtos == null)
        //    {
        //        return Problem("Erro ao criar um produto, contate o suporte!");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        //return BadRequest(ModelState);

        //        //return ValidationProblem(ModelState);

        //        return ValidationProblem(new ValidationProblemDetails(ModelState) 
        //        { 
        //            Title = "Um ou mais erros de validação ocorreram!"                    
        //        });
        //    }

        //    _positionRepository.Produtos.Add(produto);
        //    await _positionRepository.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        //}

        //[HttpPut("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> PutProduto(int id, PositionViewModel produto)
        //{
        //    if (id != produto.Id) return BadRequest();

        //    if (!ModelState.IsValid) return ValidationProblem(ModelState);

        //    _positionRepository.Entry(produto).State = EntityState.Modified;

        //    try
        //    {
        //        await _positionRepository.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProdutoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpDelete("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> DeleteProduto(int id)
        //{
        //    if (_positionRepository.Produtos == null)
        //    {
        //        return NotFound();
        //    }

        //    var produto = await _positionRepository.Produtos.FindAsync(id);

        //    if (produto == null)
        //    {
        //        return NotFound();
        //    }

        //    _positionRepository.Produtos.Remove(produto);
        //    await _positionRepository.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProdutoExists(int id)
        //{
        //    return (_positionRepository.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
