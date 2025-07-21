using CadastroPessoasApi.DTOs.V2;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CadastroPessoasApi.Controllers.v2;

[ApiVersion("2.0")]
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
    public async Task<ActionResult<PessoaJuridicaReadDtoV2>> Create(PessoaJuridicaCreateDtoV2 dto)
    {
        var endereco = new Endereco
        {
            Logradouro = dto.Endereco.Logradouro,
            Numero = dto.Endereco.Numero,
            Bairro = dto.Endereco.Bairro,
            Cidade = dto.Endereco.Cidade,
            Estado = dto.Endereco.Estado,
            CEP = dto.Endereco.CEP
        };
        var pessoa = new PessoaJuridica
        {
            Nome = dto.Nome,
            Email = dto.Email,
            CNPJ = dto.CNPJ,
            RazaoSocial = dto.RazaoSocial,
            Tipo = TipoPessoa.Juridica,
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow,
            Endereco = endereco
        };

        await _repository.AdicionarPessoaJuridica(pessoa);

        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, new PessoaJuridicaReadDtoV2
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
    public async Task<ActionResult<PessoaJuridicaReadDtoV2>> GetById(int id)
    {
        var pessoa = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoa == null) return NotFound();

        return Ok(new PessoaJuridicaReadDtoV2
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
    public async Task<ActionResult<IEnumerable<PessoaJuridicaReadDtoV2>>> GetAll()
    {
        var pessoas = await _repository.ObterTodasPessoasJuridicas();

        return Ok(pessoas.Select(p => new PessoaJuridicaReadDtoV2
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
    public async Task<IActionResult> Update(int id, PessoaJuridicaCreateDtoV2 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null)
            return NotFound();

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.CNPJ = dto.CNPJ;
        pessoaExistente.RazaoSocial = dto.RazaoSocial;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;
        
        if (pessoaExistente.Endereco is null)
            pessoaExistente.Endereco = new Endereco { IdPessoa = pessoaExistente.Id };

        pessoaExistente.Endereco.Logradouro = dto.Endereco.Logradouro;
        pessoaExistente.Endereco.Numero = dto.Endereco.Numero;
        pessoaExistente.Endereco.Bairro = dto.Endereco.Bairro;
        pessoaExistente.Endereco.Cidade = dto.Endereco.Cidade;
        pessoaExistente.Endereco.Estado = dto.Endereco.Estado;
        pessoaExistente.Endereco.CEP = dto.Endereco.CEP;
        
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
