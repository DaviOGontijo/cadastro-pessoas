using CadastroPessoasApi.DTOs.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CadastroPessoasApi.Controllers.V2
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly IPessoaFisicaServiceV2 _service;

        public PessoaFisicaController(IPessoaFisicaServiceV2 service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<PessoaFisicaReadDtoV2>> Create(PessoaFisicaCreateDtoV2 dto)
        {
            var (success, errorMessage, result) = await _service.CreateAsync(dto);

            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { id = result?.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisicaReadDtoV2>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaFisicaReadDtoV2>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PessoaFisicaCreateDtoV2 dto)
        {
            var (Success, ErrorMessage) = await _service.UpdateAsync(id, dto);
            if (!Success)
            {
                return BadRequest(ErrorMessage);
            }
            return Success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}