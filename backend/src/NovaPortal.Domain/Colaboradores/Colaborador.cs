namespace NovaPortal.Domain.Colaboradores;

// Un colaborador con acceso a NovaPortal. De la contraseña solo se guardan la sal y el hash.
public sealed record Colaborador(
    Guid Id_Colaborador,
    string Correo_Colaborador,
    string Nombre_Colaborador,
    byte[] Sal_Colaborador,
    byte[] Hash_Colaborador);
