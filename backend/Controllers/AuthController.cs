using CadastroPessoasApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private static readonly List<(string Username, string Password)> _users = new();

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] CadastroDto dto)
    {
        if (_users.Any(u => u.Username == dto.Username))
            return BadRequest("Usuário já existe.");

        _users.Add((dto.Username, dto.Password));
        return Ok("Registrado com sucesso.");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] CadastroDto dto)
    {
        if (!_users.Any(u => u.Username == dto.Username && u.Password == dto.Password))
            return Unauthorized();

        var token = _tokenService.GenerateJwtToken(dto.Username);
        return Ok(new { Token = token });
    }

    //[HttpGet("users")]
    //public IActionResult GetUsers()
    //{
    //    var users = _users.Select(u => new { u.Username }).ToList();
    //    return Ok(users);
    //}
}
