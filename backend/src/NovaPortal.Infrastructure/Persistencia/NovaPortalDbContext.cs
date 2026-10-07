using Microsoft.EntityFrameworkCore;
using NovaPortal.Domain.Colaboradores;

namespace NovaPortal.Infrastructure.Persistencia;

public sealed class NovaPortalDbContext(DbContextOptions<NovaPortalDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Colaborador> Colaboradores => Set<Colaborador>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        var colaborador = modelo.Entity<Colaborador>();
        colaborador.ToTable("colaboradores");
        colaborador.HasKey(c => c.Id_Colaborador).HasName("pk_colaboradores");
        colaborador.Property(c => c.Id_Colaborador).HasColumnName("id_colaborador");
        colaborador.Property(c => c.Correo_Colaborador).HasColumnName("correo_colaborador").HasMaxLength(150).IsRequired();
        colaborador.HasIndex(c => c.Correo_Colaborador).IsUnique().HasDatabaseName("ux_colaboradores_correo_colaborador");
        colaborador.Property(c => c.Nombre_Colaborador).HasColumnName("nombre_colaborador").HasMaxLength(100).IsRequired();
        colaborador.Property(c => c.Sal_Colaborador).HasColumnName("sal_colaborador").IsRequired();
        colaborador.Property(c => c.Hash_Colaborador).HasColumnName("hash_colaborador").IsRequired();
    }
}
