namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaJuridicaCreateDtoV2
    {
        public string Nome { get; set; } = null!;
        public string? Email { get; set; }
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
        public EnderecoDto Endereco { get; set; } = null!;
    }
}
