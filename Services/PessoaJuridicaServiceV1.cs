using CadastroPessoasApi.DTOs.V1;
using CadastroPessoasApi.Models.Enums;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Validators;

public class PessoaJuridicaServiceV1 : IPessoaJuridicaServiceV1
{
    private readonly IPessoaRepository _repository;

    public PessoaJuridicaServiceV1(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string? ErrorMessage, PessoaJuridicaReadDtoV1? Result)> CreateAsync(PessoaJuridicaCreateDtoV1 dto)
    {
        if (!CpfCnpjValidator.ValidarCNPJ(dto.CNPJ))
            return(false, "CNPJ inválido.", null);

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

        var result = new PessoaJuridicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        };
        return (true, null, result);
    }

    public async Task<PessoaJuridicaReadDtoV1?> GetByIdAsync(int id)
    {
        var pessoa = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoa == null) return null;

        return new PessoaJuridicaReadDtoV1
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            CNPJ = pessoa.CNPJ,
            RazaoSocial = pessoa.RazaoSocial,
            DataCadastro = pessoa.DataCadastro,
            DataAtualizacao = pessoa.DataAtualizacao
        };
    }

    public async Task<IEnumerable<PessoaJuridicaReadDtoV1>> GetAllAsync()
    {
        var pessoas = await _repository.ObterTodasPessoasJuridicas();

        return pessoas.Select(p => new PessoaJuridicaReadDtoV1
        {
            Id = p.Id,
            Nome = p.Nome,
            Email = p.Email,
            CNPJ = p.CNPJ,
            RazaoSocial = p.RazaoSocial,
            DataCadastro = p.DataCadastro,
            DataAtualizacao = p.DataAtualizacao
        });
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaJuridicaCreateDtoV1 dto)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null)
            return (false, "Empresa não encontrada");
        if (!CpfCnpjValidator.ValidarCNPJ(dto.CNPJ))
            return(false, "CNPJ inválido.");

        pessoaExistente.Nome = dto.Nome;
        pessoaExistente.Email = dto.Email;
        pessoaExistente.CNPJ = dto.CNPJ;
        pessoaExistente.RazaoSocial = dto.RazaoSocial;
        pessoaExistente.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarPessoaJuridica(pessoaExistente);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pessoaExistente = await _repository.ObterPessoaJuridicaPorId(id);
        if (pessoaExistente == null)
            return false;

        await _repository.DeletarPessoaJuridica(pessoaExistente);
        return true;
    }
}
