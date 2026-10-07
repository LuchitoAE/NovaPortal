using NovaPortal.Application.Colaboradores;
using NovaPortal.Application.Seguridad;
using NovaPortal.Domain.Colaboradores;

namespace NovaPortal.Application.Autenticacion;

public interface IServicioAutenticacion
{
    Colaborador? Autenticar(string correo_colaborador, string contrasena_colaborador);
}

public sealed class ServicioAutenticacion(IRepositorioColaboradores repositorio, IVerificadorContrasenas verificador)
    : IServicioAutenticacion
{
    public Colaborador? Autenticar(string correo_colaborador, string contrasena_colaborador)
    {
        var colaborador = repositorio.BuscarPorCorreo(correo_colaborador);
        var contrasena_valida = verificador.Verificar(contrasena_colaborador, colaborador);

        return colaborador is not null && contrasena_valida ? colaborador : null;
    }
}
