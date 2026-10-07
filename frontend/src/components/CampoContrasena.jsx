import { forwardRef, useState } from 'react'
import { CampoTexto } from './CampoTexto'

// RV-09: la contraseña empieza oculta; el botón la muestra u oculta y dice con texto qué hará.
// Quien usa el formulario decide la visibilidad (se vuelve a ocultar al enviar).
export const CampoContrasena = forwardRef(function CampoContrasena(
  { visible, onAlternarVisibilidad, ...propiedades },
  referencia,
) {
  const [bloqMayusActivo, setBloqMayusActivo] = useState(false)

  // Prevención de errores: avisa si Bloq Mayús está activado mientras se escribe la contraseña.
  function revisarBloqMayus(evento) {
    setBloqMayusActivo(Boolean(evento.getModifierState?.('CapsLock')))
  }

  return (
    <CampoTexto
      ref={referencia}
      type={visible ? 'text' : 'password'}
      autoComplete="current-password"
      onKeyDown={revisarBloqMayus}
      onKeyUp={revisarBloqMayus}
      onBlur={() => setBloqMayusActivo(false)}
      ayuda={bloqMayusActivo ? 'Bloq Mayús está activado: la contraseña distingue mayúsculas y minúsculas.' : undefined}
      {...propiedades}
      accion={
        <button
          type="button"
          className="boton-secundario boton-mostrar"
          aria-controls={propiedades.id}
          aria-pressed={visible}
          onClick={onAlternarVisibilidad}
        >
          {visible ? 'Ocultar' : 'Mostrar'}
        </button>
      }
    />
  )
})
