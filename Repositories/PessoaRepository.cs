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

        public async Task AdicionarPessoaFisica(PessoaFisica pessoaFisica)
        {
            _context.Pessoas.Add(pessoaFisica);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarPessoaJuridica(PessoaJuridica pessoaJuridica)
        {
            _context.Pessoas.Add(pessoaJuridica);
            await _context.SaveChangesAsync();
        }

        public async Task<PessoaFisica?> ObterPessoaFisicaPorId(int id)
        {
            return await _context.Pessoas
                .OfType<PessoaFisica>()
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PessoaFisica>> ObterTodasPessoasFisicas()
        {
            return await _context.Pessoas
                .OfType<PessoaFisica>()
                .Include(p => p.Endereco)
                .ToListAsync();
        }

        public async Task<PessoaJuridica?> ObterPessoaJuridicaPorId(int id)
        {
            return await _context.Pessoas
                .OfType<PessoaJuridica>()
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PessoaJuridica>> ObterTodasPessoasJuridicas()
        {
            return await _context.Pessoas
                .OfType<PessoaJuridica>()
                .Include(p => p.Endereco)
                .ToListAsync();
        }
        public async Task AtualizarPessoaFisica(PessoaFisica pessoaFisica)
        {
            _context.Pessoas.Update(pessoaFisica);
            await _context.SaveChangesAsync();
        }
        public async Task AtualizarPessoaJuridica(PessoaJuridica pessoaJuridica)
        {
            _context.Pessoas.Update(pessoaJuridica);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarPessoaFisica(PessoaFisica pessoaFisica)
        {
            _context.Pessoas.Remove(pessoaFisica);
            await _context.SaveChangesAsync();
        }
        public async Task DeletarPessoaJuridica(PessoaJuridica pessoaJuridica)
        {
            _context.Pessoas.Remove(pessoaJuridica);
            await _context.SaveChangesAsync();
        }
    }
}
