using CadastroPessoasApi.DTOs.V1;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CadastroPessoasApi.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PessoaJuridicaController : ControllerBase
{
    private readonly IPessoaRepository _repository;

    public PessoaJuridicaController(IPessoaRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<PessoaJuridicaReadDtoV1>> Create(PessoaJuridicaCreateDtoV1 dto)
    {
        var pessoa = new PessoaJuridica
        {
            Nome = dto.Nome,
            Email = dto.Email,
            CNPJ = dto.CNPJ,
            RazaoSocial = dto.RazaoSocial,
            Tipo = TipoPessoa.Juridica,
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _repository.AdicionarPessoaJuridica(pessoa);

        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, new PessoaJuridicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaJuridicaReadDtoV1>> GetById(int id)
    {
        var pessoa = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoa == null) return NotFound();

        return Ok(new PessoaJuridicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaJuridicaReadDtoV1>>> GetAll()
    {
        var pessoas = await _repository.ObterTodasPessoasJuridicas();

        return Ok(pessoas.Select(p => new PessoaJuridicaReadDtoV1
        {
            Id = p.Id,
            Nome = p.Nome,
            Email = p.Email,
            CNPJ = p.CNPJ,
            RazaoSocial = p.RazaoSocial,
            DataCadastro = p.DataCadastro,
            DataAtualizacao = p.DataAtualizacao
        }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PessoaJuridicaCreateDtoV1 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null)
            return NotFound();

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.CNPJ = dto.CNPJ;
        pessoaExistente.RazaoSocial = dto.RazaoSocial;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarPessoaJuridica(pessoaExistente);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null)
            return NotFound();

        await _repository.DeletarPessoaJuridica(pessoaExistente);

        return NoContent();
    }
}
