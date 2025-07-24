using CadastroPessoasApi.Models;

namespace CadastroPessoasApi.Repositories
{
    public interface IPessoaRepository
    {
        Task AdicionarPessoaFisica(Pessoa pessoa);

        Task<Pessoa> ObterPessoaFisicaPorId(int id);
        Task<IEnumerable<Pessoa>> ObterTodasPessoasFisicas();

        Task AtualizarPessoaFisica(Pessoa pessoa);

        Task DeletarPessoaFisica(Pessoa pessoa);

        Task<Pessoa> ObterPessoaFisicaPorCpf(string cpf);
    }
}
