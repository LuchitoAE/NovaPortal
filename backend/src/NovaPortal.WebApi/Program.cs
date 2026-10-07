using NovaPortal.Application;
using NovaPortal.Infrastructure;
using NovaPortal.WebApi.Autenticacion;
using NovaPortal.WebApi.Json;

var builder = WebApplication.CreateBuilder(args);

// La cadena de conexión no está en el código: en desarrollo vive en los secretos de usuario (user-secrets).
var cadena_conexion = builder.Configuration.GetConnectionString("NovaPortal")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexión «NovaPortal». Configúrela con dotnet user-secrets (ver el README).");

// El JSON usa los nombres de las propiedades en minúsculas: correo_colaborador, contrasena_colaborador.
builder.Services.ConfigureHttpJsonOptions(opciones =>
    opciones.SerializerOptions.PropertyNamingPolicy = new Nombres_En_Minusculas());
builder.Services.AddProblemDetails();

// Punto de composición: aquí se conectan las interfaces de Application con sus implementaciones.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(cadena_conexion);

// Descripción de la API (/openapi/v1.json) para Swagger.
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

// Swagger solo en desarrollo: sirve para probar el endpoint a mano en /swagger.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opciones => opciones.SwaggerEndpoint("/openapi/v1.json", "NovaPortal v1"));
}

app.MapAutenticacion();

app.Run();
