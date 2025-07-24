using System.Threading.Tasks;
using Xunit;
using Moq;
using CadastroPessoasApi.Services;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.DTOs.V2;
using CadastroPessoasApi.Models;
using System;

public class PessoaFisicaServiceV2Tests
{
    private readonly Mock<IPessoaRepository> _repoMock;
    private readonly PessoaFisicaServiceV2 _service;

    public PessoaFisicaServiceV2Tests()
    {
        _repoMock = new Mock<IPessoaRepository>();
        _service = new PessoaFisicaServiceV2(_repoMock.Object);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoCpfForVazio()
    {
        var dto = new PessoaFisicaCreateDtoV2 { Nome = "Teste", CPF = "" };

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("CPF é obrigatório.", erro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.ObterPessoaFisicaPorCpf(It.IsAny<string>()), Times.Never);
        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoCpfForInvalido()
    {
        var dto = new PessoaFisicaCreateDtoV2 { Nome = "Teste", CPF = "123" };

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("CPF inválido.", erro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.ObterPessoaFisicaPorCpf(It.IsAny<string>()), Times.Never);
        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoCpfJaExistir()
    {
        var dto = new PessoaFisicaCreateDtoV2 { Nome = "Teste", CPF = "83571390008" };
        var pessoaExistente = new Pessoa { CPF = dto.CPF };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync(pessoaExistente);

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("CPF já cadastrado.", erro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Fact]
    public async Task Criar_DeveRetornarSucesso_QuandoDadosForemValidos()
    {
        var dto = new PessoaFisicaCreateDtoV2
        {
            Nome = "Davi",
            CPF = "83571390008",
            Endereco = new EnderecoDto
            {
                Logradouro = "Rua A",
                Numero = "123",
                Bairro = "Bairro X",
                Cidade = "Cidade Y",
                Estado = "Estado Z",
                CEP = "12345000"
            }
        };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync((Pessoa?)null);

        _repoMock.Setup(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()))
                 .Returns(Task.CompletedTask);

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.True(sucesso);
        Assert.Null(erro);
        Assert.NotNull(resultado);
        Assert.Equal(dto.Nome, resultado!.Nome);
        Assert.Equal(dto.CPF, resultado.CPF);
        Assert.Equal(dto.Endereco.Logradouro, resultado.Endereco!.Logradouro);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Once);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoNomeForVazio()
    {
        var dto = new PessoaFisicaCreateDtoV2
        {
            Nome = "",
            CPF = "83571390008",
            Endereco = new EnderecoDto
            {
                Logradouro = "Rua A",
                Numero = "123",
                Bairro = "Bairro X",
                Cidade = "Cidade Y",
                Estado = "Estado Z",
                CEP = "12345000"
            }
        };

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("Nome é obrigatório.", erro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoAdicionarPessoaLancarExcecao()
    {
        var dto = new PessoaFisicaCreateDtoV2
        {
            Nome = "Davi",
            CPF = "83571390008",
            Endereco = new EnderecoDto
            {
                Logradouro = "Rua A",
                Numero = "123",
                Bairro = "Bairro X",
                Cidade = "Cidade Y",
                Estado = "Estado Z",
                CEP = "12345000"
            }
        };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync((Pessoa?)null);

        _repoMock.Setup(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()))
                 .ThrowsAsync(new Exception("Falha no banco de dados"));

        var (sucesso, erro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Contains("Falha ao cadastrar pessoa:", erro);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarErro_QuandoPessoaNaoExistir()
    {
        var dto = new PessoaFisicaCreateDtoV2
        {
            Nome = "Atualizado",
            CPF = "83571390008",
            Endereco = new EnderecoDto
            {
                Logradouro = "Rua Atualizada",
                Numero = "456",
                Bairro = "Bairro Atualizado",
                Cidade = "Cidade Atualizada",
                Estado = "Estado Atualizado",
                CEP = "65432100"
            }
        };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorId(It.IsAny<int>()))
                 .ReturnsAsync((Pessoa?)null);

        var (sucesso, erro) = await _service.UpdateAsync(1, dto);

        Assert.False(sucesso);
        Assert.Equal("Pessoa não encontrada.", erro);
    }
}
