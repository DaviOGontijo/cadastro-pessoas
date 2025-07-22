using CadastroPessoasApi.DTOs.V1;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Validators;

public class PessoaFisicaServiceV1 : IPessoaFisicaServiceV1
{
    private readonly IPessoaRepository _repository;

    public PessoaFisicaServiceV1(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string? ErrorMessage, PessoaFisicaReadDtoV1? Result)> CreateAsync(PessoaFisicaCreateDtoV1 dto)
    {
        var cpfExistente = await _repository.ObterPessoaFisicaPorCpf(dto.CPF);

        if (!CpfCnpjValidator.ValidarCPF(dto.CPF))
            return (false, "CPF inválido.", null);
        if (cpfExistente != null)
            return (false, "CPF já cadastrado.", null);

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

        var result = new PessoaFisicaReadDtoV1
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
        };

        return (true, null, result);
    }

    public async Task<PessoaFisicaReadDtoV1?> GetByIdAsync(int id)
    {
        var pessoa = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoa == null) return null;

        return new PessoaFisicaReadDtoV1
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
        };
    }

    public async Task<IEnumerable<PessoaFisicaReadDtoV1>> GetAllAsync()
    {
        var pessoas = await _repository.ObterTodasPessoasFisicas();
        return pessoas.Select(p => new PessoaFisicaReadDtoV1
        {
            Id = p.Id,
            Nome = p.Nome,
            Sexo = p.Sexo,
            Email = p.Email,
            DataNascimento = p.DataNascimento,
            Naturalidade = p.Naturalidade,
            Nacionalidade = p.Nacionalidade,
            CPF = p.CPF,
            DataCadastro = p.DataCadastro,
            DataAtualizacao = p.DataAtualizacao
        });
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaFisicaCreateDtoV1 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoaExistente == null)
            return (false, "Pessoa não encontrada.");

        var cpfExistente = await _repository.ObterPessoaFisicaPorCpf(dto.CPF);
        if (cpfExistente != null && cpfExistente.Id != id)
            return (false, "CPF já cadastrado para outra pessoa.");
        if (!CpfCnpjValidator.ValidarCPF(dto.CPF))
            return (false, "CPF inválido.");

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Sexo = dto.Sexo;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.DataNascimento = dto.DataNascimento;
        pessoaExistente.Naturalidade = dto.Naturalidade;
        pessoaExistente.Nacionalidade = dto.Nacionalidade;
        pessoaExistente.CPF = dto.CPF;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarPessoaFisica(pessoaExistente);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pessoa = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoa == null) return false;

        await _repository.DeletarPessoaFisica(pessoa);
        return true;
    }
}
