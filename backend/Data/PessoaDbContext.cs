using CadastroPessoasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroPessoasApi.Data;

public class PessoaDbContext : DbContext
{
    public PessoaDbContext(DbContextOptions<PessoaDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas { get; set; } = null!;
    public DbSet<Endereco> Enderecos { get; set; } = null!;
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>()
            .HasOne(p => p.Endereco)
            .WithOne(e => e.Pessoa)
            .HasForeignKey<Endereco>(e => e.IdPessoa)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
