using NovaPortal.Domain.Colaboradores;

namespace NovaPortal.Application.Colaboradores;

public interface IRepositorioColaboradores
{
    Colaborador? BuscarPorCorreo(string correo_colaborador);
}
