import { Aviso } from './Aviso'

// Cierre del diálogo: confirma el ingreso y ofrece la acción contraria (salir).
export function Bienvenida({ nombre_colaborador, onCerrarSesion }) {
  return (
    <section className="bienvenida">
      <Aviso tipo="exito" titulo={`Hola, ${nombre_colaborador}.`}>
        Ingresó correctamente a NovaPortal.
      </Aviso>
      <button type="button" className="boton-secundario" onClick={onCerrarSesion}>
        Cerrar sesión
      </button>
    </section>
  )
}
