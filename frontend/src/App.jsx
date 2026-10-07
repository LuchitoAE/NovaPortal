import { useState } from 'react'
import { Bienvenida } from './components/Bienvenida'
import { FormularioIngreso } from './components/FormularioIngreso'

export default function App() {
  const [nombre_colaborador, setNombreColaborador] = useState('')

  return (
    <div className="pagina">
      {/* Identidad institucional: en escritorio es un panel a la izquierda; en el móvil, la cabecera. */}
      <header className="panel-marca">
        <p className="marca">NovaPortal</p>
        <p className="lema">Acceso institucional</p>
      </header>

      <div className="columna-formulario">
        <main className="tarjeta">
          {nombre_colaborador ? (
            <Bienvenida nombre_colaborador={nombre_colaborador} onCerrarSesion={() => setNombreColaborador('')} />
          ) : (
            <>
              <h1>Iniciar sesión</h1>
              <p className="subtitulo">Use su cuenta de NovaTech.</p>
              <FormularioIngreso onIngreso={setNombreColaborador} />
            </>
          )}
        </main>
        <footer className="pie">NovaTech</footer>
      </div>
    </div>
  )
}
