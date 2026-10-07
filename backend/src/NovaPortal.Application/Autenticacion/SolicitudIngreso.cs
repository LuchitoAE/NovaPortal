namespace NovaPortal.Application.Autenticacion;

// Lo que envía el formulario.
public sealed record SolicitudIngreso(string? Correo_Colaborador, string? Contrasena_Colaborador);

// Lo que devuelve un ingreso correcto: solo el nombre para saludar, nada sensible.
public sealed record RespuestaIngreso(string Nombre_Colaborador);
