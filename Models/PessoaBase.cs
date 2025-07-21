using CadastroPessoasApi.Models.Enums;

namespace CadastroPessoasApi.Models;

public abstract class PessoaBase
{
    public int Id { get; set; }
    public TipoPessoa Tipo { get; set; }
    public string Nome { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }

    public Endereco? Endereco { get; set; }
}
