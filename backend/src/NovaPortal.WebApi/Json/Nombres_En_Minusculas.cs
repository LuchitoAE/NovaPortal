using System.Text.Json;

namespace NovaPortal.WebApi.Json;

// Correo_Colaborador → correo_colaborador. El frontend usa los mismos nombres.
public sealed class Nombres_En_Minusculas : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLowerInvariant();
}
