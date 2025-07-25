﻿using CadastroPessoasApi.Services;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PessoaDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService, PessoaDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CadastroDto dto)
    {
        var normalizedUsername = dto.Username.ToLower();

        var exists = await _context.Users.AnyAsync(u => u.Username.ToLower() == normalizedUsername);
        if (exists)
            return BadRequest("Usuário já existe.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User { Username = normalizedUsername, Password = hashedPassword };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registrado com sucesso.");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CadastroDto dto)
    {
        var normalizedUsername = dto.Username.ToLower();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUsername);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized();

        var token = _tokenService.GenerateJwtToken(user.Username); 
        return Ok(new { Token = token });
    }
}
