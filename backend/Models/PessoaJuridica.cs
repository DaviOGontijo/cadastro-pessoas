namespace CadastroPessoasApi.Models;

public class PessoaJuridica : PessoaBase
{
    public string CNPJ { get; set; } = null!;
    public string? RazaoSocial { get; set; }
}
