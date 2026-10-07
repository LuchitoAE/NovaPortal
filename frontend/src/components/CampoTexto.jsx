import { forwardRef } from 'react'

// Etiqueta, control, ayuda y error, siempre en el mismo orden. El error queda asociado al control.
export const CampoTexto = forwardRef(function CampoTexto(
  { id, etiqueta, ayuda, error, accion, ...propiedades },
  referencia,
) {
  const descritoPor = [error && `${id}-error`, ayuda && `${id}-ayuda`].filter(Boolean).join(' ')

  return (
    <div className="campo">
      <label htmlFor={id}>{etiqueta}</label>
      <div className="control">
        <input
          ref={referencia}
          id={id}
          name={id}
          aria-invalid={Boolean(error)}
          aria-describedby={descritoPor || undefined}
          {...propiedades}
        />
        {accion}
      </div>
      {ayuda && (
        <p id={`${id}-ayuda`} className="ayuda" aria-live="polite">
          {ayuda}
        </p>
      )}
      {error && (
        <p id={`${id}-error`} className="error">
          {error}
        </p>
      )}
    </div>
  )
})
