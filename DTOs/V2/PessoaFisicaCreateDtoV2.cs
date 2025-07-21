namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaFisicaCreateDtoV2
    {
        public string Nome { get; set; } = null!;
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        public string CPF { get; set; } = null!;
        public EnderecoDto Endereco { get; set; } = null!;
    }
}
