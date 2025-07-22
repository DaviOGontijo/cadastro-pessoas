using CadastroPessoasApi.DTOs.V1;
using Microsoft.AspNetCore.Mvc;
using CadastroPessoasApi.Validators;

namespace CadastroPessoasApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PessoaJuridicaController : ControllerBase
    {
        private readonly IPessoaJuridicaServiceV1 _service;

        public PessoaJuridicaController(IPessoaJuridicaServiceV1 service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<PessoaJuridicaReadDtoV1>> Create(PessoaJuridicaCreateDtoV1 dto)
        {
            var (success, errorMessage, result) = await _service.CreateAsync(dto);

            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaJuridicaReadDtoV1>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaJuridicaReadDtoV1>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PessoaJuridicaCreateDtoV1 dto)
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