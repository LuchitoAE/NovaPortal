# NovaPortal — formulario de validación de credenciales

Caso práctico de la Unidad 2 (Construcción de Software, UPLA). Formulario web de inicio de sesión con React + Vite y una API en ASP.NET Core (.NET 10), con arquitectura limpia en cuatro proyectos, que valida las credenciales contra PostgreSQL.

## Base de datos (PostgreSQL)

Base `novaportal`, rol `novaportal` y tabla `colaboradores`, creada por las migraciones de Entity Framework Core. La tabla empieza vacía: los colaboradores se agregan con psql (ver más abajo). La clave del rol está en `~/.pgpass` y en los secretos de usuario de la API (`dotnet user-secrets`); **nunca en el código ni en este README**.

Para crearla en otra máquina (fish):

```fish
# 1. Rol y base, con tu usuario de PostgreSQL
psql -d postgres -c "CREATE ROLE novaportal LOGIN PASSWORD 'elija-una-clave';"
psql -d postgres -c "GRANT novaportal TO $USER;"     # PostgreSQL 16+: para poder crear la base a su nombre
psql -d postgres -c "CREATE DATABASE novaportal OWNER novaportal;"

# 2. Cadena de conexión fuera del código
cd NovaPortal/backend
dotnet user-secrets set "ConnectionStrings:NovaPortal" "Host=localhost;Port=5432;Database=novaportal;Username=novaportal;Password=elija-una-clave" --project src/NovaPortal.WebApi

# 3. Tabla colaboradores (migraciones)
dotnet tool restore
env ASPNETCORE_ENVIRONMENT=Development dotnet ef database update --project src/NovaPortal.Infrastructure --startup-project src/NovaPortal.WebApi
```

`ASPNETCORE_ENVIRONMENT=Development` hace falta para que `dotnet ef` lea los secretos de usuario.

### Consultas SQL

```fish
psql -h localhost -U novaportal -d novaportal
```

```sql
\d colaboradores
SELECT correo_colaborador, nombre_colaborador FROM colaboradores;
SELECT correo_colaborador, length(hash_colaborador) AS bytes_hash FROM colaboradores;  -- solo hay hash, nunca la contraseña
\q
```

## Cómo ejecutarlo

Se necesitan dos terminales:

```fish
# 1. API (http://localhost:5300; Swagger en http://localhost:5300/swagger)
cd NovaPortal/backend
dotnet run --project src/NovaPortal.WebApi --launch-profile http

# 2. Formulario (http://localhost:5180)
cd NovaPortal/frontend
pnpm install        # solo la primera vez
pnpm dev
```

Para detenerlos, Ctrl+C en cada terminal.

## Agregar o quitar colaboradores (psql)

La tabla no guarda la contraseña, sino su sal y su hash PBKDF2 (SHA-256, 600 000 iteraciones, 32 bytes). Por eso un colaborador se agrega en dos pasos.

**1. Calcular la sal y el hash** (Python pide la contraseña sin mostrarla):

```fish
python3 -c 'import base64,getpass,hashlib,os; c=getpass.getpass("Contraseña: "); s=os.urandom(16); h=hashlib.pbkdf2_hmac("sha256",c.encode(),s,600000,32); print(base64.b64encode(s).decode(), base64.b64encode(h).decode())'
```

**2. Insertarlo en psql** con los dos valores que imprimió (primero la sal, después el hash):

```sql
insert into colaboradores (id_colaborador, correo_colaborador, nombre_colaborador, sal_colaborador, hash_colaborador)
values (gen_random_uuid(), 'nombre.apellido@novatech.com.pe', 'Nombre Apellido',
        decode('SAL', 'base64'), decode('HASH', 'base64'));
```

El correo va en minúsculas, porque la API lo compara así. Para quitar uno: `delete from colaboradores where correo_colaborador = '…';`. Si la base se vuelve a crear con las migraciones, la tabla queda vacía y hay que volver a agregarlos.

## Probar la API

- **Swagger:** http://localhost:5300/swagger → `POST /api/autenticacion/ingreso` → Try it out → Execute.
- **VS Code:** `backend/src/NovaPortal.WebApi/NovaPortal.WebApi.http` (extensión REST Client) trae los escenarios E-01, E-02, E-04 y E-05.

| Respuesta | Cuándo |
|---|---|
| 200 `{ "nombre_colaborador": … }` | Credenciales correctas |
| 400 con `errors` por campo | Campos vacíos, correo mal formado o de otro dominio |
| 401 con un mensaje que no dice qué dato falló | Correo inexistente o contraseña incorrecta |

## Estructura (arquitectura limpia)

```
backend/src/
  NovaPortal.Domain/          Colaborador (no depende de nada)
  NovaPortal.Application/     ReglasCredenciales, ServicioAutenticacion, solicitud y respuesta,
                              interfaces IRepositorioColaboradores e IVerificadorContrasenas
  NovaPortal.Infrastructure/  NovaPortalDbContext, migraciones, usuarios iniciales,
                              RepositorioColaboradoresPostgres, VerificadorContrasenasPbkdf2
  NovaPortal.WebApi/          Program.cs (punto de composición), AutenticacionEndpoints, Swagger
frontend/src/
  validacion/credenciales.js  reglas RV-01 a RV-04, sin interfaz
  api/autenticacion.js        única llamada a la API
  components/                 CampoTexto, CampoContrasena, Aviso, FormularioIngreso, Bienvenida
```

Dependencias: Application → Domain; Infrastructure → Application y Domain; WebApi → Application e Infrastructure (esta última solo para registrar servicios en `Program.cs`). Diagrama: `Diagramas/Arquitectura de NovaPortal.drawio`.
