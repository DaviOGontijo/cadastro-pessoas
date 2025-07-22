using CadastroPessoasApi.DTOs.V2;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Validators;

public class PessoaFisicaServiceV2 : IPessoaFisicaServiceV2
{
    private readonly IPessoaRepository _repository;

    public PessoaFisicaServiceV2(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string? ErrorMessage, PessoaFisicaReadDtoV2? Result)> CreateAsync(PessoaFisicaCreateDtoV2 dto)
    {
        var cpfExistente = await _repository.ObterPessoaFisicaPorCpf(dto.CPF);
        if (cpfExistente != null)
            return (false, "CPF já cadastrado.", null);
        if (!CpfCnpjValidator.ValidarCPF(dto.CPF))
            return (false, "CPF inválido.", null);

        var endereco = new Endereco
        {
            Logradouro = dto.Endereco.Logradouro,
            Numero = dto.Endereco.Numero,
            Complemento = dto.Endereco.Complemento,
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

        var result = new PessoaFisicaReadDtoV2
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
            DataAtualizacao = pessoa.DataAtualizacao,
            Endereco = dto.Endereco
        };
        return (true, null, result);
    }

    public async Task<PessoaFisicaReadDtoV2?> GetByIdAsync(int id)
    {
        var pessoa = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoa == null) return null;

        var enderecoDto = pessoa.Endereco is null ? null : new EnderecoDto
        {
            Logradouro = pessoa.Endereco.Logradouro,
            Numero = pessoa.Endereco.Numero,
            Bairro = pessoa.Endereco.Bairro,
            Cidade = pessoa.Endereco.Cidade,
            Estado = pessoa.Endereco.Estado,
            CEP = pessoa.Endereco.CEP
        };

        return new PessoaFisicaReadDtoV2
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
            DataAtualizacao = pessoa.DataAtualizacao,
            Endereco = enderecoDto
        };
    }
    public async Task<IEnumerable<PessoaFisicaReadDtoV2>> GetAllAsync()
    {
        var pessoas = await _repository.ObterTodasPessoasFisicas();

        return pessoas.Select(p => new PessoaFisicaReadDtoV2
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
            DataAtualizacao = p.DataAtualizacao,
            Endereco = p.Endereco == null ? null : new EnderecoDto
            {
                Logradouro = p.Endereco.Logradouro,
                Numero = p.Endereco.Numero,
                Complemento = p.Endereco.Complemento,
                Bairro = p.Endereco.Bairro,
                Cidade = p.Endereco.Cidade,
                Estado = p.Endereco.Estado,
                CEP = p.Endereco.CEP
            }
        });
    }
    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaFisicaCreateDtoV2 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoaExistente == null) return (false, "Pessoa não encontrada.");

        var cpfExistente = await _repository.ObterPessoaFisicaPorCpf(dto.CPF);
        if (cpfExistente != null && cpfExistente.Id != id)
            return (false, "CPF já cadastrado para outra pessoa.");
        if (!CpfCnpjValidator.ValidarCPF(dto.CPF))
            return(false,"CPF inválido.");

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Sexo = dto.Sexo;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.DataNascimento = dto.DataNascimento;
        pessoaExistente.Naturalidade = dto.Naturalidade;
        pessoaExistente.Nacionalidade = dto.Nacionalidade;
        pessoaExistente.CPF = dto.CPF;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;

        if (pessoaExistente.Endereco == null)
            pessoaExistente.Endereco = new Endereco { IdPessoa = pessoaExistente.Id };

        pessoaExistente.Endereco.Logradouro = dto.Endereco.Logradouro;
        pessoaExistente.Endereco.Numero = dto.Endereco.Numero;
        pessoaExistente.Endereco.Complemento = dto.Endereco.Complemento;
        pessoaExistente.Endereco.Bairro = dto.Endereco.Bairro;
        pessoaExistente.Endereco.Cidade = dto.Endereco.Cidade;
        pessoaExistente.Endereco.Estado = dto.Endereco.Estado;
        pessoaExistente.Endereco.CEP = dto.Endereco.CEP;

        await _repository.AtualizarPessoaFisica(pessoaExistente);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pessoaExistente = await _repository.ObterPessoaFisicaPorId(id);
        if (pessoaExistente == null) return false;

        await _repository.DeletarPessoaFisica(pessoaExistente);
        return true;
    }
}
