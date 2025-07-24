namespace CadastroPessoasApi.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        public string Logradouro { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Complemento { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string CEP { get; set; } = null!;

        public int IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; } = null!;
    }
}
