using CadastroPessoasApi.Models;

namespace CadastroPessoasApi.Repositories
{
    public interface IPessoaRepository
    {
        Task AdicionarPessoaFisica(PessoaFisica pessoaFisica);
        Task AdicionarPessoaJuridica(PessoaJuridica pessoaJuridica);

        Task<PessoaFisica?> ObterPessoaFisicaPorId(int id);
        Task<IEnumerable<PessoaFisica>> ObterTodasPessoasFisicas();

        Task<PessoaJuridica?> ObterPessoaJuridicaPorId(int id);
        Task<IEnumerable<PessoaJuridica>> ObterTodasPessoasJuridicas();

        Task AtualizarPessoaFisica(PessoaFisica pessoaFisica);
        Task AtualizarPessoaJuridica(PessoaJuridica pessoaJuridica);

        Task DeletarPessoaFisica(PessoaFisica pessoaFisica);
        Task DeletarPessoaJuridica(PessoaJuridica pessoaJuridica);
    }
}
