using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaJuridicaCreateDtoV2
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; } = null!;
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
        [Required(ErrorMessage = "Endereço é obrigatório.")]
        public EnderecoDto Endereco { get; set; } = null!;
    }
}
