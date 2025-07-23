using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaFisicaCreateDtoV2
    {
        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        public string Nome { get; set; } = null!;
        public string? Sexo { get; set; }
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        [Required(ErrorMessage = "CPF é obrigatório.")]
        public string CPF { get; set; } = null!;

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        public EnderecoDto Endereco { get; set; } = null!;
    }
}
