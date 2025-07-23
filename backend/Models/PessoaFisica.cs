namespace CadastroPessoasApi.Models;

public class PessoaFisica : PessoaBase
{
    public string? Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Naturalidade { get; set; }
    public string? Nacionalidade { get; set; }
    public string CPF { get; set; } = null!;
}
