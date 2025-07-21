using CadastroPessoasApi.Models;
using CadastroPessoasApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CadastroPessoasApi.Data;

public class PessoaDbContext : DbContext
{
    public PessoaDbContext(DbContextOptions<PessoaDbContext> options) : base(options) { }

    public DbSet<PessoaBase> Pessoas { get; set; } = null!;
    public DbSet<Endereco> Enderecos { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PessoaBase>()
            .HasDiscriminator(p => p.Tipo)
            .HasValue<PessoaFisica>(TipoPessoa.Fisica)
            .HasValue<PessoaJuridica>(TipoPessoa.Juridica);

        modelBuilder.Entity<PessoaBase>()
            .HasOne(p => p.Endereco)
            .WithOne(e => e.Pessoa)
            .HasForeignKey<Endereco>(e => e.IdPessoa)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
