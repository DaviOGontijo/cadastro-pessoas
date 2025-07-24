using System.Threading.Tasks;
using Xunit;
using Moq;
using CadastroPessoasApi.Services;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.DTOs.V1;
using CadastroPessoasApi.Models;
using System;

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
    public async Task Criar_DeveRetornarSucesso_QuandoCpfNaoExistir()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "83571390008" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync((Pessoa?)null);

        _repoMock.Setup(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()))
                 .Returns(Task.CompletedTask);

        var (sucesso, mensagemErro, resultado) = await _service.CreateAsync(dto);

        Assert.True(sucesso);
        Assert.Null(mensagemErro);
        Assert.NotNull(resultado);
        Assert.Equal(dto.Nome, resultado.Nome);
        Assert.Equal(dto.CPF, resultado.CPF);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Once);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoCpfJaExistir()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = "83571390008" };
        var pessoaExistente = new Pessoa { CPF = dto.CPF, Nome = "Fulano" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync(pessoaExistente);

        var (sucesso, mensagemErro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("CPF já cadastrado.", mensagemErro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    public async Task Criar_DeveRetornarErro_QuandoCpfForInvalido(string cpfInvalido)
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Fulano", CPF = cpfInvalido };

        var (sucesso, mensagemErro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Equal("CPF é obrigatório.", mensagemErro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.ObterPessoaFisicaPorCpf(It.IsAny<string>()), Times.Never);
        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Never);
    }

    [Fact]
    public async Task Criar_DeveRetornarErro_QuandoAdicionarPessoaLancarExcecao()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Erro Teste", CPF = "83571390008" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync((Pessoa?)null);

        _repoMock.Setup(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()))
                 .ThrowsAsync(new Exception("Falha no banco de dados"));

        var (sucesso, mensagemErro, resultado) = await _service.CreateAsync(dto);

        Assert.False(sucesso);
        Assert.Contains("Falha ao cadastrar pessoa:", mensagemErro);
        Assert.Null(resultado);

        _repoMock.Verify(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()), Times.Once);
    }

    [Fact]
    public async Task Criar_DeveRetornarPessoaComNomeCorreto()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Maria Silva", CPF = "83571390008" };

        _repoMock.Setup(r => r.ObterPessoaFisicaPorCpf(dto.CPF))
                 .ReturnsAsync((Pessoa?)null);

        _repoMock.Setup(r => r.AdicionarPessoaFisica(It.IsAny<Pessoa>()))
                 .Returns(Task.CompletedTask);

        var (sucesso, mensagemErro, resultado) = await _service.CreateAsync(dto);

        Assert.True(sucesso);
        Assert.Equal("Maria Silva", resultado.Nome);
    }
}
