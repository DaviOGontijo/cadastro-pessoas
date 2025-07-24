using Xunit;
using Moq;
using CadastroPessoasApi.Controllers.V1;
using CadastroPessoasApi.Services;
using CadastroPessoasApi.DTOs.V1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PessoaFisicaControllerTests
{
    private readonly Mock<IPessoaFisicaServiceV1> _serviceMock;
    private readonly PessoaFisicaController _controller;

    public PessoaFisicaControllerTests()
    {
        _serviceMock = new Mock<IPessoaFisicaServiceV1>();
        _controller = new PessoaFisicaController(_serviceMock.Object);
    }

    [Fact]
    public async Task Criar_RetornaCreated_QuandoServicoTemSucesso()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Teste", CPF = "83571390008" };
        var readDto = new PessoaFisicaReadDtoV1 { Id = 1, Nome = "Teste", CPF = "83571390008" };

        _serviceMock.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync((true, null, readDto));

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(PessoaFisicaController.GetById), createdResult.ActionName);
        Assert.Equal(readDto, createdResult.Value);
    }

    [Fact]
    public async Task Criar_RetornaBadRequest_QuandoServicoFalha()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Teste", CPF = "invalid" };

        _serviceMock.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync((false, "CPF inválido.", null));

        var result = await _controller.Create(dto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("CPF inválido.", badRequestResult.Value);
    }

    [Fact]
    public async Task ObterPorId_RetornaOk_QuandoPessoaExiste()
    {
        var readDto = new PessoaFisicaReadDtoV1 { Id = 1, Nome = "Teste" };

        _serviceMock.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(readDto);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(readDto, okResult.Value);
    }

    [Fact]
    public async Task ObterPorId_RetornaNotFound_QuandoPessoaNaoExiste()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync((PessoaFisicaReadDtoV1?)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task ObterTodos_RetornaOk_ComLista()
    {
        var lista = new List<PessoaFisicaReadDtoV1>
        {
            new PessoaFisicaReadDtoV1 { Id = 1, Nome = "Teste 1" },
            new PessoaFisicaReadDtoV1 { Id = 2, Nome = "Teste 2" }
        };

        _serviceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(lista);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lista, okResult.Value);
    }

    [Fact]
    public async Task Atualizar_RetornaNoContent_QuandoSucesso()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Atualizado", CPF = "83571390008" };

        _serviceMock.Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync((true, null));

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Atualizar_RetornaBadRequest_QuandoErro()
    {
        var dto = new PessoaFisicaCreateDtoV1 { Nome = "Atualizado", CPF = "invalid" };

        _serviceMock.Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync((false, "CPF inválido."));

        var result = await _controller.Update(1, dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("CPF inválido.", badRequest.Value);
    }

    [Fact]
    public async Task Deletar_RetornaNoContent_QuandoSucesso()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Deletar_RetornaNotFound_QuandoNaoEncontrado()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(false);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
