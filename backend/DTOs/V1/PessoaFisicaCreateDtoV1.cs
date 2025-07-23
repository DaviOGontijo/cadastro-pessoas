using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.DTOs.V1
{
    public class PessoaFisicaCreateDtoV1
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; } 
        public string? Sexo { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }
       
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório.")]
        public string CPF { get; set; }
    }

}
