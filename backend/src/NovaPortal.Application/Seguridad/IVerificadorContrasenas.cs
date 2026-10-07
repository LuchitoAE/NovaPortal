using NovaPortal.Domain.Colaboradores;

namespace NovaPortal.Application.Seguridad;

// Comprueba una contraseña contra lo guardado del colaborador, sin que la aplicación sepa cómo se calcula el hash.
public interface IVerificadorContrasenas
{
    // Si no hay colaborador igual se calcula un hash: la respuesta tarda lo mismo y no revela qué correos existen.
    bool Verificar(string contrasena_colaborador, Colaborador? colaborador);
}
