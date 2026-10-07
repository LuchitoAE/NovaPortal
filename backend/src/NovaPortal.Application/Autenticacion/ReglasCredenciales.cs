using System.Text.RegularExpressions;

namespace NovaPortal.Application.Autenticacion;

// Las mismas reglas que el formulario (RV-01 a RV-04): el servidor no confía en lo que llega del navegador.
public static partial class ReglasCredenciales
{
    public const string Dominio_Institucional = "@novatech.com.pe";

    public static string NormalizarCorreo(string? correo_colaborador) =>
        (correo_colaborador ?? string.Empty).Trim().ToLowerInvariant();

    public static Dictionary<string, string[]> Validar(SolicitudIngreso solicitud)
    {
        var errores = new Dictionary<string, string[]>();
        var correo = NormalizarCorreo(solicitud.Correo_Colaborador);

        if (correo.Length == 0)
        {
            errores["correo_colaborador"] = ["Escriba su correo institucional."];
        }
        else if (!FormatoCorreo().IsMatch(correo))
        {
            errores["correo_colaborador"] = ["Escriba el correo completo, por ejemplo: nombre.apellido@novatech.com.pe."];
        }
        else if (!correo.EndsWith(Dominio_Institucional, StringComparison.Ordinal))
        {
            errores["correo_colaborador"] = ["Use su correo institucional, que termina en @novatech.com.pe."];
        }

        if (string.IsNullOrEmpty(solicitud.Contrasena_Colaborador))
        {
            errores["contrasena_colaborador"] = ["Escriba su contraseña."];
        }

        return errores;
    }

    [GeneratedRegex(@"^[a-z0-9._%+-]+@[a-z0-9-]+(\.[a-z0-9-]+)*\.[a-z]{2,}$")]
    private static partial Regex FormatoCorreo();
}
