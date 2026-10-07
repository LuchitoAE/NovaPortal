import { useRef, useState } from 'react'
import { ingresar } from '../api/autenticacion'
import {
  CREDENCIALES_VACIAS,
  EJEMPLO_CORREO,
  normalizarCorreo,
  validarCampo,
  validarCredenciales,
} from '../validacion/credenciales'
import { Aviso } from './Aviso'
import { CampoContrasena } from './CampoContrasena'
import { CampoTexto } from './CampoTexto'

export function FormularioIngreso({ onIngreso }) {
  const [credenciales, setCredenciales] = useState(CREDENCIALES_VACIAS)
  const [errores, setErrores] = useState({})
  const [validando, setValidando] = useState(false)
  const [avisoError, setAvisoError] = useState('')
  const [contrasenaVisible, setContrasenaVisible] = useState(false)
  const campoCorreo = useRef(null)
  const campoContrasena = useRef(null)

  function cambiar(campo, valor) {
    setCredenciales((anteriores) => ({ ...anteriores, [campo]: valor }))
    // Un error no desaparece al teclear: se actualiza y solo se quita cuando el dato ya es correcto.
    if (errores[campo]) {
      setErrores((anteriores) => ({ ...anteriores, [campo]: validarCampo(campo, valor) }))
    }
  }

  async function enviar(evento) {
    evento.preventDefault()
    if (validando) {
      return // RV-08: no se envían de nuevo las mismas credenciales mientras se validan.
    }

    setAvisoError('')
    setContrasenaVisible(false) // la contraseña no queda a la vista después de enviar
    const encontrados = validarCredenciales(credenciales)
    setErrores(encontrados)
    const primerError = Object.keys(encontrados)[0]
    if (primerError) {
      // RV-07: con errores no se envía; el foco va al primer campo que hay que corregir.
      const campo = primerError === 'correo_colaborador' ? campoCorreo : campoContrasena
      campo.current.focus()
      return
    }

    setValidando(true)
    const resultado = await ingresar({
      ...credenciales,
      correo_colaborador: normalizarCorreo(credenciales.correo_colaborador),
    })
    setValidando(false)

    if (resultado.tipo === 'exito') {
      onIngreso(resultado.nombre_colaborador)
    } else if (resultado.tipo === 'errores_de_campo') {
      setErrores(resultado.errores)
      const campo = resultado.errores.correo_colaborador ? campoCorreo : campoContrasena
      campo.current.focus()
    } else {
      setAvisoError(resultado.mensaje)
      // El foco vuelve a la contraseña, ya seleccionada, para corregirla sin buscar dónde quedó.
      campoContrasena.current.focus()
      campoContrasena.current.select()
    }
  }

  return (
    <form className="formulario" onSubmit={enviar} noValidate aria-busy={validando}>
      <CampoTexto
        ref={campoCorreo}
        id="correo_colaborador"
        etiqueta="Correo institucional"
        type="email"
        inputMode="email"
        autoComplete="username"
        autoCapitalize="none"
        autoCorrect="off"
        spellCheck={false}
        autoFocus
        value={credenciales.correo_colaborador}
        onChange={(evento) => cambiar('correo_colaborador', evento.target.value)}
        ayuda={`Ejemplo: ${EJEMPLO_CORREO}`}
        error={errores.correo_colaborador}
        readOnly={validando}
      />

      <CampoContrasena
        ref={campoContrasena}
        id="contrasena_colaborador"
        etiqueta="Contraseña"
        value={credenciales.contrasena_colaborador}
        onChange={(evento) => cambiar('contrasena_colaborador', evento.target.value)}
        error={errores.contrasena_colaborador}
        readOnly={validando}
        visible={contrasenaVisible}
        onAlternarVisibilidad={() => setContrasenaVisible((anterior) => !anterior)}
      />

      {avisoError && (
        <Aviso tipo="error" titulo="No se pudo iniciar sesión.">
          {avisoError}
        </Aviso>
      )}

      <button type="submit" className="boton-principal" aria-disabled={validando} aria-describedby="ayuda_ingresar">
        {validando ? 'Validando…' : 'Ingresar'}
      </button>
      <p id="ayuda_ingresar" className="ayuda">
        El botón se desactiva mientras se validan las credenciales.
      </p>
      {validando && (
        <p className="ayuda" role="status">
          Estamos validando sus credenciales; espere un momento.
        </p>
      )}
    </form>
  )
}
