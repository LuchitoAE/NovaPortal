using Microsoft.Extensions.DependencyInjection;
using NovaPortal.Application.Autenticacion;

namespace NovaPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection servicios) =>
        servicios.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();
}
