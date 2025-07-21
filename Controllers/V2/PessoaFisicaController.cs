using CadastroPessoasApi.Models;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.DTOs.V2;
using Microsoft.AspNetCore.Mvc;
using CadastroPessoasApi.Repositories;

namespace CadastroPessoasApi.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PessoaFisicaController : ControllerBase
{
    private readonly IPessoaRepository _repository;

    public PessoaFisicaController(IPessoaRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<PessoaFisicaReadDtoV2>> Create(PessoaFisicaCreateDtoV2 dto)
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
            DataAtualizacao = DateTime.UtcNow,
            Endereco = endereco
        };

        await _repository.AdicionarPessoaFisica(pessoa);

        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, new PessoaFisicaReadDtoV2
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
    public async Task<ActionResult<PessoaFisicaReadDtoV2>> GetById(int id)
    {
        var pessoa = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoa == null) return NotFound();

        return Ok(new PessoaFisicaReadDtoV2
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
    //public async Task<ActionResult<IEnumerable<PessoaFisicaReadDtoV2>>> GetAll()
    //{
    //    var pessoas = await _repository.ObterTodasPessoasFisicas();
    //    return Ok(pessoas.Select(p => new PessoaFisicaReadDtoV2
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
    public async Task<IActionResult> Update(int id, PessoaFisicaCreateDtoV2 dto)
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

        if (pessoaExistente.Endereco is null)
            pessoaExistente.Endereco = new Endereco { IdPessoa = pessoaExistente.Id };

        pessoaExistente.Endereco.Logradouro = dto.Endereco.Logradouro;
        pessoaExistente.Endereco.Numero = dto.Endereco.Numero;
        pessoaExistente.Endereco.Bairro = dto.Endereco.Bairro;
        pessoaExistente.Endereco.Cidade = dto.Endereco.Cidade;
        pessoaExistente.Endereco.Estado = dto.Endereco.Estado;
        pessoaExistente.Endereco.CEP = dto.Endereco.CEP;

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
