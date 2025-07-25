using System.Threading.Tasks;
using CadastroPessoasApi.Controllers;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CadastroPessoasApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly SqliteConnection _connection;
        private readonly PessoaDbContext _context;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<PessoaDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new PessoaDbContext(options);
            _context.Database.EnsureCreated();

            _tokenServiceMock = new Mock<ITokenService>();

            _controller = new AuthController(_tokenServiceMock.Object, _context);
        }

        [Fact]
        public async Task Registro_DeveRegistrarUsuario_QuandoUsernameNaoExistir()
        {
            var dto = new CadastroDto { Username = "TesteUser", Password = "Senha123" };

            var result = await _controller.Register(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Registrado com sucesso.", okResult.Value);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username.ToLower());
            Assert.NotNull(userInDb);
            Assert.NotEqual(dto.Password, userInDb.Password);
        }

        [Fact]
        public async Task Registro_DeveRetornarBadRequest_QuandoUsuarioJaExistir()
        {
            var dto = new CadastroDto { Username = "existente", Password = "123" };

            _context.Users.Add(new User { Username = dto.Username.ToLower(), Password = "qualquer" });
            await _context.SaveChangesAsync();

            var result = await _controller.Register(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário já existe.", badRequest.Value);
        }

        [Theory]
        [InlineData("wronguser", "Senha123")]
        [InlineData("loginuser", "wrongpass")]
        public async Task Login_DeveRetornarUnauthorized_QuandoCredenciaisInvalidas(string username, string password)
        {
            var validUser = new User
            {
                Username = "loginuser",
                Password = BCrypt.Net.BCrypt.HashPassword("Senha123")
            };
            _context.Users.Add(validUser);
            await _context.SaveChangesAsync();

            var dto = new CadastroDto { Username = username, Password = password };

            var result = await _controller.Login(dto);

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
