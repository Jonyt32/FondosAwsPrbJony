// src/app/app.routes.server.ts
import { ServerRoute, RenderMode } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Rutas raíz/públicas
  { path: '',               renderMode: RenderMode.Client }, // equivale a '/'
  { path: 'login',          renderMode: RenderMode.Client },
  { path: 'unauthorized',   renderMode: RenderMode.Client },

  // Wildcard
  { path: '**',             renderMode: RenderMode.Client },

  // Rutas del feature Clientes
  { path: 'clientes',                       renderMode: RenderMode.Client },
  { path: 'clientes/listar-cliente',        renderMode: RenderMode.Client },
  { path: 'clientes/crear-cliente',         renderMode: RenderMode.Client },
  { path: 'clientes/cliente-transacciones', renderMode: RenderMode.Client },
  { path: 'clientes/detalle-cliente/:id',   renderMode: RenderMode.Client }, // dinámica

  // Rutas del feature Usuarios
  { path: 'usuarios',               renderMode: RenderMode.Client },
  { path: 'usuarios/listar-usuario',renderMode: RenderMode.Client },
  { path: 'usuarios/registrar-usuario', renderMode: RenderMode.Client },

  // Rutas del feature Fondos
  { path: 'fondos', renderMode: RenderMode.Client },
  // agrega subrutas si las tienes
];
