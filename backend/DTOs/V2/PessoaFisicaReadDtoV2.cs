namespace CadastroPessoasApi.DTOs.V2
{
    public class PessoaFisicaReadDtoV2
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        public string CPF { get; set; } = null!;
        public EnderecoDto Endereco { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
