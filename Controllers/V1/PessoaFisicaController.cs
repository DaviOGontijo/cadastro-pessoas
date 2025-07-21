using CadastroPessoasApi.Models;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.DTOs.V1;
using Microsoft.AspNetCore.Mvc;
using CadastroPessoasApi.Repositories;

namespace CadastroPessoasApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PessoaFisicaController : ControllerBase
{
    private readonly IPessoaRepository _repository;

    public PessoaFisicaController(IPessoaRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<PessoaFisicaReadDtoV1>> Create(PessoaFisicaCreateDtoV1 dto)
    {
        var pessoa = new PessoaFisica
        {
            Nome = dto.Nome,
            Sexo = dto.Sexo,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Naturalidade = dto.Naturalidade,
            Nacionalidade = dto.Nacionalidade,
            CPF = dto.CPF,
            Tipo = TipoPessoa.Fisica,
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _repository.AdicionarPessoaFisica(pessoa);

        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, new PessoaFisicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Sexo = pessoa.Sexo,
            Email = pessoa.Email,
            DataNascimento = pessoa.DataNascimento,
            Naturalidade = pessoa.Naturalidade,
            Nacionalidade = pessoa.Nacionalidade,
            CPF = pessoa.CPF,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaFisicaReadDtoV1>> GetById(int id)
    {
        var pessoa = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoa == null) return NotFound();

        return Ok(new PessoaFisicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Sexo = pessoa.Sexo,
            Email = pessoa.Email,
            DataNascimento = pessoa.DataNascimento,
            Naturalidade = pessoa.Naturalidade,
            Nacionalidade = pessoa.Nacionalidade,
            CPF = pessoa.CPF,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        });
    }

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<PessoaFisicaReadDtoV1>>> GetAll()
    //{
    //    var pessoas = await _repository.ObterTodasPessoasFisicas();
    //    return Ok(pessoas.Select(p => new PessoaFisicaReadDtoV1
    //    {
    //        Id = p.Id,
    //        Nome = p.Nome,
    //        Sexo = p.Sexo,
    //        Email = p.Email,
    //        DataNascimento = p.DataNascimento,
    //        Naturalidade = p.Naturalidade,
    //        Nacionalidade = p.Nacionalidade,
    //        CPF = p.CPF,
    //        DataCadastro = p.DataCadastro,
    //        DataAtualizacao = p.DataAtualizacao
    //    }));
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PessoaFisicaCreateDtoV1 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoaExistente == null)
            return NotFound();

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Sexo = dto.Sexo;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.DataNascimento = dto.DataNascimento;
        pessoaExistente.Naturalidade = dto.Naturalidade;
        pessoaExistente.Nacionalidade = dto.Nacionalidade;
        pessoaExistente.CPF = dto.CPF;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarPessoaFisica(pessoaExistente);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoaExistente = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoaExistente == null)
            return NotFound();

        await _repository.DeletarPessoaFisica(pessoaExistente);

        return NoContent();
    }
}
