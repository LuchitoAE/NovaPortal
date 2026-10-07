import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// En desarrollo el formulario se sirve en el puerto 5180 (ConstruSoft usa el 5173)
// y /api se reenvía a la API de NovaPortal (puerto 5300).
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5180,
    strictPort: true,
    proxy: {
      '/api': 'http://localhost:5300',
    },
  },
})
