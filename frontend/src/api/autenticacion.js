// Único lugar que habla con la API. Convierte cada respuesta en un resultado que el formulario entiende.
const RUTA_INGRESO = '/api/autenticacion/ingreso'

export async function ingresar(credenciales) {
  let respuesta
  try {
    respuesta = await fetch(RUTA_INGRESO, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credenciales),
    })
  } catch {
    return { tipo: 'sin_conexion', mensaje: 'No se pudo conectar con NovaPortal. Revise su conexión e inténtelo de nuevo.' }
  }

  const datos = await respuesta.json().catch(() => null)

  if (respuesta.ok) {
    return { tipo: 'exito', nombre_colaborador: datos.nombre_colaborador }
  }

  if (respuesta.status === 400 && datos?.errors) {
    // El servidor encontró errores de formato: se muestran junto a cada campo.
    const errores = Object.fromEntries(Object.entries(datos.errors).map(([campo, mensajes]) => [campo, mensajes[0]]))
    return { tipo: 'errores_de_campo', errores }
  }

  if (respuesta.status === 401) {
    return { tipo: 'credenciales_no_validas', mensaje: datos?.detail }
  }

  return { tipo: 'error_inesperado', mensaje: 'NovaPortal no pudo validar sus datos en este momento. Inténtelo de nuevo en unos minutos.' }
}
