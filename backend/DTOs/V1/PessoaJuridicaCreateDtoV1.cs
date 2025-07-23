using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.DTOs.V1
{
    public class PessoaJuridicaCreateDtoV1
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; } = null!;

        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        public string CNPJ { get; set; } = null!;
        public string? RazaoSocial { get; set; }
    }
}
