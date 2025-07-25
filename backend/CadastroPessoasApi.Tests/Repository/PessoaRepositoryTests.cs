using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroPessoasApi.Tests.Repositories
{
    public class PessoaRepositoryTests : IDisposable
    {
        private readonly PessoaDbContext _context;
        private readonly PessoaRepository _repository;
        private readonly SqliteConnection _connection;

        public PessoaRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<PessoaDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new PessoaDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new PessoaRepository(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _connection?.Dispose();
        }

        private Pessoa CriarPessoaFake()
        {
            return new Pessoa
            {
                Nome = "João Teste",
                CPF = "12345678900",
                Email = "joao@teste.com",
                DataNascimento = new DateTime(1990, 1, 1),
                Sexo = "M",
                Naturalidade = "Brasília",
                Nacionalidade = "Brasileiro",
                Endereco = new Endereco
                {
                    Logradouro = "Rua A",
                    Numero = "123",
                    Bairro = "Centro",
                    Cidade = "Brasília",
                    Estado = "DF",
                    CEP = "70000000"
                }
            };
        }

        [Fact]
        public async Task AdicionarPessoaFisica_DeveAdicionar()
        {
            var pessoa = CriarPessoaFake();

            await _repository.AdicionarPessoaFisica(pessoa);

            var pessoaNoBanco = await _context.Pessoas.FirstOrDefaultAsync(p => p.CPF == pessoa.CPF);

            Assert.NotNull(pessoaNoBanco);
            Assert.Equal("João Teste", pessoaNoBanco.Nome);
        }

        [Fact]
        public async Task ObterPessoaFisicaPorId_DeveRetornarPessoa()
        {
            var pessoa = CriarPessoaFake();
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var pessoaDb = await _repository.ObterPessoaFisicaPorId(pessoa.Id);

            Assert.NotNull(pessoaDb);
            Assert.Equal(pessoa.CPF, pessoaDb.CPF);
        }

        [Fact]
        public async Task ObterTodasPessoasFisicas_DeveRetornarLista()
        {
            _context.Pessoas.Add(CriarPessoaFake());
            _context.Pessoas.Add(CriarPessoaFake());
            await _context.SaveChangesAsync();

            var pessoas = await _repository.ObterTodasPessoasFisicas();

            Assert.NotEmpty(pessoas);
            Assert.Equal(2, pessoas.Count());
        }

        [Fact]
        public async Task AtualizarPessoaFisica_DeveAtualizar()
        {
            var pessoa = CriarPessoaFake();
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            pessoa.Nome = "Nome Atualizado";
            await _repository.AtualizarPessoaFisica(pessoa);

            var pessoaAtualizada = await _context.Pessoas.FindAsync(pessoa.Id);

            Assert.Equal("Nome Atualizado", pessoaAtualizada.Nome);
        }

        [Fact]
        public async Task DeletarPessoaFisica_DeveRemover()
        {
            var pessoa = CriarPessoaFake();
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            await _repository.DeletarPessoaFisica(pessoa);

            var pessoaNoBanco = await _context.Pessoas.FindAsync(pessoa.Id);

            Assert.Null(pessoaNoBanco);
        }

        [Fact]
        public async Task ObterPessoaFisicaPorCpf_DeveRetornarCorreto()
        {
            var pessoa = CriarPessoaFake();
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var resultado = await _repository.ObterPessoaFisicaPorCpf(pessoa.CPF);

            Assert.NotNull(resultado);
            Assert.Equal(pessoa.Nome, resultado.Nome);
        }
    }
}
