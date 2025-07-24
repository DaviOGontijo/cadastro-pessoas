using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroPessoasApi.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly PessoaDbContext _context;

        public PessoaRepository(PessoaDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarPessoaFisica(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
        }

        public async Task<Pessoa> ObterPessoaFisicaPorId(int id)
        {
            return await _context.Pessoas
                .OfType<Pessoa>()
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pessoa>> ObterTodasPessoasFisicas()
        {
            return await _context.Pessoas
                .OfType<Pessoa>()
                .Include(p => p.Endereco)
                .ToListAsync();
        }

        public async Task AtualizarPessoaFisica(Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
            await _context.SaveChangesAsync();
        }
        public async Task DeletarPessoaFisica(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
        public async Task<Pessoa> ObterPessoaFisicaPorCpf(string cpf)
        {
            return await _context.Pessoas
                .OfType<Pessoa>()
                .FirstOrDefaultAsync(p => p.CPF == cpf);
        }
    }
}
