namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaJuridicaReadDtoV2
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Email { get; set; }
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
        public EnderecoDto Endereco { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
