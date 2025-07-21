namespace CadastroPessoasApi.DTOs.V1
{
    public class PessoaJuridicaCreateDtoV1
    {
        public string Nome { get; set; } = null!;
        public string? Email { get; set; }
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
    }
}
