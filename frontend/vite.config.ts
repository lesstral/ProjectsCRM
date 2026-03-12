import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 63232,
        proxy: {
      '/api': {
        target: 'https://localhost:7224', // backend url
        changeOrigin: true,
        secure: false
          },
      '/api-alt': { target: 'https://localhost:5195', changeOrigin: true, secure: false }    
    }
  }
})

