import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { ListarFondos } from './features/fondos/listar-fondos/listar-fondos';
import { AuthGuard } from './services/core/auth-guard';
import { RoleGuard } from './services/core/role-guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'clientes',
        loadChildren: () =>
          import('./features/clientes/clientes-module').then(m => m.ClientesModule),
        canActivate: [RoleGuard],
        data: { roles: ['Admin', 'User'] }
      },
      {
        path: 'fondos',
        loadChildren: () =>
          import('./features/fondos/fondos-module').then(m => m.FondosModule),
        canActivate: [RoleGuard],
        data: { roles: ['User'] }
      },
      {
        path: 'usuarios',
        loadChildren: () =>
          import('./features/usuarios/usuarios-module').then(m => m.UsuariosModule),
        canActivate: [RoleGuard],
        data: { roles: ['Admin'] }
      }
    ]
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: '**',
    redirectTo: 'clientes'
  }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
