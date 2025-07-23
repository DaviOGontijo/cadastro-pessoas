namespace CadastroPessoasApi.DTOs.V1
{
    public class PessoaJuridicaReadDtoV1
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Email { get; set; }
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
