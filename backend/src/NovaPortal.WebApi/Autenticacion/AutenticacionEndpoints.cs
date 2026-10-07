using NovaPortal.Application.Autenticacion;

namespace NovaPortal.WebApi.Autenticacion;

public static class AutenticacionEndpoints
{
    // RV-10: el mismo mensaje para correo inexistente y contraseña errada; no dice cuál de los dos falló.
    public const string Mensaje_Credenciales_No_Validas =
        "El correo o la contraseña no son correctos. Revise e inténtelo de nuevo.";

    public static void MapAutenticacion(this WebApplication app) =>
        app.MapPost("/api/autenticacion/ingreso", Ingresar)
            .WithTags("Autenticación")
            .WithSummary("Valida el correo institucional y la contraseña de un colaborador.")
            .Produces<RespuestaIngreso>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

    private static IResult Ingresar(SolicitudIngreso solicitud, IServicioAutenticacion servicio)
    {
        var errores = ReglasCredenciales.Validar(solicitud);
        if (errores.Count > 0)
        {
            return Results.ValidationProblem(errores, title: "Revise los datos del formulario.");
        }

        var colaborador = servicio.Autenticar(
            ReglasCredenciales.NormalizarCorreo(solicitud.Correo_Colaborador),
            solicitud.Contrasena_Colaborador!);

        return colaborador is null
            ? Results.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "No se pudo iniciar sesión.",
                detail: Mensaje_Credenciales_No_Validas)
            : Results.Ok(new RespuestaIngreso(colaborador.Nombre_Colaborador));
    }
}
