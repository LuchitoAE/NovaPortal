// Reglas de validación del formulario (RV-01 a RV-04), separadas de la presentación.
// La API vuelve a aplicar las mismas reglas: aquí solo sirven para avisar antes de enviar.
export const DOMINIO_INSTITUCIONAL = '@novatech.com.pe'
export const EJEMPLO_CORREO = 'nombre.apellido@novatech.com.pe'

const FORMATO_CORREO = /^[a-z0-9._%+-]+@[a-z0-9-]+(\.[a-z0-9-]+)*\.[a-z]{2,}$/

export const CREDENCIALES_VACIAS = { correo_colaborador: '', contrasena_colaborador: '' }

// RV-04: los espacios del inicio y del final no son parte del correo.
export function normalizarCorreo(correo) {
  return correo.trim().toLowerCase()
}

export function validarCorreo(correo) {
  const normalizado = normalizarCorreo(correo)

  if (!normalizado) {
    return 'Escriba su correo institucional.'
  }

  if (!FORMATO_CORREO.test(normalizado)) {
    return `Escriba el correo completo, por ejemplo: ${EJEMPLO_CORREO}.`
  }

  if (!normalizado.endsWith(DOMINIO_INSTITUCIONAL)) {
    return `Use su correo institucional, que termina en ${DOMINIO_INSTITUCIONAL}.`
  }

  return ''
}

export function validarContrasena(contrasena) {
  return contrasena ? '' : 'Escriba su contraseña.'
}

const VALIDADORES = {
  correo_colaborador: validarCorreo,
  contrasena_colaborador: validarContrasena,
}

export function validarCampo(campo, valor) {
  return VALIDADORES[campo](valor)
}

// Devuelve solo los campos con error, en el orden del formulario.
export function validarCredenciales(credenciales) {
  const errores = {}
  for (const campo of Object.keys(VALIDADORES)) {
    const error = validarCampo(campo, credenciales[campo])
    if (error) {
      errores[campo] = error
    }
  }
  return errores
}
