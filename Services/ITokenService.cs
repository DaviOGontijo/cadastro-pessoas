namespace CadastroPessoasApi.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
    }
}
