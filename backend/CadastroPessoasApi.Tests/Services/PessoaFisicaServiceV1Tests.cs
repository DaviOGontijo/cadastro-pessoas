using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroPessoasApi.DTOs.V1;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Services;
using Moq;
using Xunit;

public class PessoaFisicaServiceV1Tests
{
    private readonly Mock<IPessoaRepository> _repoMock;
    private readonly PessoaFisicaServiceV1 _service;

    public PessoaFisicaServiceV1Tests()
    {
        _repoMock = new Mock<IPessoaRepository>();
        _service = new PessoaFisicaServiceV1(_repoMock.Object);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoCpfInvalido()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "123" };

        var (success, errorMessage, result) = await _service.CreateAsync(dto);

        Assert.False(success);
        Assert.Equal("CPF inválido.", errorMessage);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetById_DeveRetornarPessoa_QuandoIdExistir()
    {
        var pessoa = new Pessoa
        {
            Id = 1,
            Nome = "Fulano",
            CPF = "12345678900",
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(1)).ReturnsAsync(pessoa);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(pessoa.Nome, result.Nome);
    }

    [Fact]
    public async Task GetById_DeveRetornarNull_QuandoIdNaoExistir()
    {
        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(It.IsAny<int>())).ReturnsAsync((Pessoa)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDePessoas()
    {
        var pessoas = new List<Pessoa>
        {
            new Pessoa { Id = 1, Nome = "Fulano", CPF = "123", DataCadastro = DateTime.UtcNow, DataAtualizacao = DateTime.UtcNow },
            new Pessoa { Id = 2, Nome = "Beltrano", CPF = "456", DataCadastro = DateTime.UtcNow, DataAtualizacao = DateTime.UtcNow }
        };

        _repoMock.Setup(r => r.ObterTodasPessoasFisicas()).ReturnsAsync(pessoas);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Update_DeveRetornarErro_QuandoPessoaNaoExistir()
    {
        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(It.IsAny<int>())).ReturnsAsync((Pessoa)null);

        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "123" };

        var (success, error) = await _service.UpdateAsync(1, dto);

        Assert.False(success);
        Assert.Equal("Pessoa não encontrada.", error);
    }

    [Fact]
    public async Task Update_DeveRetornarErro_QuandoCpfPertenceAOutraPessoa()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "123" };

        var pessoa = new Pessoa { Id = 1, CPF = "123" };
        var outraPessoa = new Pessoa { Id = 2, CPF = "123" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(1)).ReturnsAsync(pessoa);
        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf("123")).ReturnsAsync(outraPessoa);

        var (success, error) = await _service.UpdateAsync(1, dto);

        Assert.False(success);
        Assert.Equal("CPF já cadastrado para outra pessoa.", error);
    }

    [Fact]
    public async Task Update_DeveRetornarErro_QuandoCpfInvalido()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "123" };
        var pessoa = new Pessoa { Id = 1, CPF = "000" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(1)).ReturnsAsync(pessoa);
        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf("123")).ReturnsAsync((Pessoa)null);

        var (success, error) = await _service.UpdateAsync(1, dto);

        Assert.False(success);
        Assert.Equal("CPF inválido.", error);
    }

    [Fact]
    public async Task Update_DeveAtualizarPessoa_QuandoDadosValidos()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Novo Nome", CPF = "12345678909" };
        var pessoa = new Pessoa { Id = 1, CPF = "000" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(1)).ReturnsAsync(pessoa);
        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf("12345678909")).ReturnsAsync((Pessoa)null);
        _repoMock.Setup(r => r.AtualizarPessoaFisica(It.IsAny<Pessoa>())).Returns(Task.CompletedTask);

        var (success, error) = await _service.UpdateAsync(1, dto);

        Assert.True(success);
        Assert.Null(error);
        _repoMock.Verify(r => r.AtualizarPessoaFisica(It.IsAny<Pessoa>()), Times.Once);
    }

    [Fact]
    public async Task Delete_DeveRetornarFalse_QuandoPessoaNaoExistir()
    {
        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(It.IsAny<int>())).ReturnsAsync((Pessoa)null);

        var result = await _service.DeleteAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task Delete_DeveRemoverPessoa_QuandoPessoaExistir()
    {
        var pessoa = new Pessoa { Id = 1 };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(1)).ReturnsAsync(pessoa);
        _repoMock.Setup(r => r.DeletarPessoaFisica(pessoa)).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
        _repoMock.Verify(r => r.DeletarPessoaFisica(pessoa), Times.Once);
    }
}
