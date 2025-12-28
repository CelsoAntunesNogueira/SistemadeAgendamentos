using Microsoft.EntityFrameworkCore;
using SalaoAPI.Models;

namespace SalaoAPI.Data;

public class SalaoContext : DbContext
{
    public SalaoContext(DbContextOptions<SalaoContext> options) : base(options)
    {
    }

    // DbSets - Tabelas do banco
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Profissional> Profissionais { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<ProfissionalServico> ProfissionalServicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração de índices e relacionamentos

        // Cliente - Índice único no telefone
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Telefone)
            .IsUnique();

        // Agendamento - Índices compostos para melhor performance
        modelBuilder.Entity<Agendamento>()
            .HasIndex(a => a.DataHora);

        modelBuilder.Entity<Agendamento>()
            .HasIndex(a => a.Status);

        modelBuilder.Entity<Agendamento>()
            .HasIndex(a => new { a.ProfissionalId, a.DataHora });

        // ProfissionalServico - Índice único composto
        modelBuilder.Entity<ProfissionalServico>()
            .HasIndex(ps => new { ps.ProfissionalId, ps.ServicoId })
            .IsUnique();

        // Configuração de relacionamentos

        // Cliente -> Agendamentos (1:N)
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Agendamentos)
            .WithOne(a => a.Cliente)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Profissional -> Agendamentos (1:N)
        modelBuilder.Entity<Profissional>()
            .HasMany(p => p.Agendamentos)
            .WithOne(a => a.Profissional)
            .HasForeignKey(a => a.ProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Servico -> Agendamentos (1:N)
        modelBuilder.Entity<Servico>()
            .HasMany(s => s.Agendamentos)
            .WithOne(a => a.Servico)
            .HasForeignKey(a => a.ServicoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Profissional <-> Servico (N:N através de ProfissionalServico)
        modelBuilder.Entity<ProfissionalServico>()
            .HasOne(ps => ps.Profissional)
            .WithMany(p => p.ProfissionalServicos)
            .HasForeignKey(ps => ps.ProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfissionalServico>()
            .HasOne(ps => ps.Servico)
            .WithMany(s => s.ProfissionalServicos)
            .HasForeignKey(ps => ps.ServicoId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}