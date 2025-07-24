namespace CadastroPessoasApi.Models;

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Email { get; set; }
    public string? Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Naturalidade { get; set; }
    public string? Nacionalidade { get; set; }
    public string CPF { get; set; } = null!;
    public Endereco? Endereco { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }


}
