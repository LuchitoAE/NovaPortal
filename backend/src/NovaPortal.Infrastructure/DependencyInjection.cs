using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NovaPortal.Application.Colaboradores;
using NovaPortal.Application.Seguridad;
using NovaPortal.Infrastructure.Colaboradores;
using NovaPortal.Infrastructure.Persistencia;
using NovaPortal.Infrastructure.Seguridad;

namespace NovaPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection servicios, string cadena_conexion)
    {
        servicios.AddDbContext<NovaPortalDbContext>(opciones => opciones.UseNpgsql(cadena_conexion));
        servicios.AddScoped<IRepositorioColaboradores, RepositorioColaboradoresPostgres>();
        servicios.AddSingleton<IVerificadorContrasenas, VerificadorContrasenasPbkdf2>();
        return servicios;
    }
}
