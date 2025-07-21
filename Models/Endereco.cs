namespace CadastroPessoasApi.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        public string Logradouro { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string CEP { get; set; } = null!;

        // FK para Pessoa
        public int IdPessoa { get; set; }
        public PessoaBase Pessoa { get; set; } = null!;
    }
}
