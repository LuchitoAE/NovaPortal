// Aviso general. El de error se anuncia de inmediato a los lectores de pantalla (role="alert").
export function Aviso({ tipo, titulo, children }) {
  return (
    <div className={`aviso aviso-${tipo}`} role={tipo === 'error' ? 'alert' : 'status'}>
      {titulo && <p className="aviso-titulo">{titulo}</p>}
      <p>{children}</p>
    </div>
  )
}
