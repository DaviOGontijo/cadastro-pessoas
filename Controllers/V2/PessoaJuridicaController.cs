using CadastroPessoasApi.DTOs.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CadastroPessoasApi.Controllers.V2
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PessoaJuridicaController : ControllerBase
    {
        private readonly IPessoaJuridicaServiceV2 _service;

        public PessoaJuridicaController(IPessoaJuridicaServiceV2 service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<PessoaJuridicaReadDtoV2>> Create(PessoaJuridicaCreateDtoV2 dto)
        {
            var (success, errorMessage, result) = await _service.CreateAsync(dto);

            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaJuridicaReadDtoV2>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaJuridicaReadDtoV2>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PessoaJuridicaCreateDtoV2 dto)
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
