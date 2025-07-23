using CadastroPessoasApi.DTOs.V2;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Validators;

public class PessoaJuridicaServiceV2 : IPessoaJuridicaServiceV2
{
    private readonly IPessoaRepository _repository;

    public PessoaJuridicaServiceV2(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string? ErrorMessage, PessoaJuridicaReadDtoV2? Result)> CreateAsync(PessoaJuridicaCreateDtoV2 dto)
    {
        var cnpjExistente = await _repository.ObterPessoaJuridicaPorCnpj(dto.CNPJ);
        if (cnpjExistente != null)
            return (false, "CNPJ já cadastrado.", null);
        if (!CpfCnpjValidator.ValidarCNPJ(dto.CNPJ))
            return(false, "CNPJ inválido.", null);

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

        var result = new PessoaJuridicaReadDtoV2
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao,
            Endereco = dto.Endereco
        };
        return (true, null, result);
    }

    public async Task<PessoaJuridicaReadDtoV2?> GetByIdAsync(int id)
    {
        var pessoa = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoa == null) return null;

        var enderecoDto = pessoa.Endereco is null ? null : new EnderecoDto
        {
            Logradouro = pessoa.Endereco.Logradouro,
            Numero = pessoa.Endereco.Numero,
            Complemento = pessoa.Endereco.Complemento,
            Bairro = pessoa.Endereco.Bairro,
            Cidade = pessoa.Endereco.Cidade,
            Estado = pessoa.Endereco.Estado,
            CEP = pessoa.Endereco.CEP
        };

        return new PessoaJuridicaReadDtoV2
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao,
            Endereco = enderecoDto
        };
    }

    public async Task<IEnumerable<PessoaJuridicaReadDtoV2>> GetAllAsync()
    {
        var pessoas = await _repository.ObterTodasPessoasJuridicas();
        return pessoas.Select(p => new PessoaJuridicaReadDtoV2
        {
            Id = p.Id,
            Nome = p.Nome,
            Email = p.Email,
            CNPJ = p.CNPJ,
            RazaoSocial = p.RazaoSocial,
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

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaJuridicaCreateDtoV2 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null) return (false, "Empresa não encontrada");

        var cnpjExistente = await _repository.ObterPessoaJuridicaPorCnpj(dto.CNPJ);
        if (cnpjExistente != null && cnpjExistente.Id != id)
            return (false, "CNPJ já cadastrado para outra empresa.");
        if (!CpfCnpjValidator.ValidarCNPJ(dto.CNPJ))
            return(false, "CNPJ inválido.");

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.CNPJ = dto.CNPJ;
        pessoaExistente.RazaoSocial = dto.RazaoSocial;
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

        await _repository.AtualizarPessoaJuridica(pessoaExistente);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null) return false;

        await _repository.DeletarPessoaJuridica(pessoaExistente);
        return true;
    }
}
