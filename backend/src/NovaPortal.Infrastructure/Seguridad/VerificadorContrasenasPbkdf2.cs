using System.Security.Cryptography;
using NovaPortal.Application.Seguridad;
using NovaPortal.Domain.Colaboradores;

namespace NovaPortal.Infrastructure.Seguridad;

// Las contraseñas nunca se guardan: solo su hash PBKDF2-HMAC-SHA256 con una sal propia por colaborador.
public sealed class VerificadorContrasenasPbkdf2 : IVerificadorContrasenas
{
    public const int Iteraciones = 600_000;
    public const int Longitud_Hash = 32;

    private static readonly byte[] Sal_Ficticia = RandomNumberGenerator.GetBytes(16);
    private static readonly byte[] Hash_Ficticio = new byte[Longitud_Hash];

    public bool Verificar(string contrasena_colaborador, Colaborador? colaborador)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            contrasena_colaborador,
            colaborador?.Sal_Colaborador ?? Sal_Ficticia,
            Iteraciones,
            HashAlgorithmName.SHA256,
            Longitud_Hash);

        // Compara en tiempo constante, para no dar pistas por la duración de la comparación.
        return CryptographicOperations.FixedTimeEquals(hash, colaborador?.Hash_Colaborador ?? Hash_Ficticio);
    }
}
