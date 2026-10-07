using Microsoft.EntityFrameworkCore;
using NovaPortal.Application.Colaboradores;
using NovaPortal.Domain.Colaboradores;
using NovaPortal.Infrastructure.Persistencia;

namespace NovaPortal.Infrastructure.Colaboradores;

// Lee los colaboradores de la tabla colaboradores de PostgreSQL.
public sealed class RepositorioColaboradoresPostgres(NovaPortalDbContext contexto) : IRepositorioColaboradores
{
    public Colaborador? BuscarPorCorreo(string correo_colaborador) =>
        contexto.Colaboradores
            .AsNoTracking()
            .FirstOrDefault(colaborador => colaborador.Correo_Colaborador == correo_colaborador);
}
