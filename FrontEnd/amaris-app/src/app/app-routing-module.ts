import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { ListarFondos } from './features/fondos/listar-fondos/listar-fondos';
import { AuthGuard } from './services/core/auth-guard';

const routes: Routes = [
  { path: 'login', component: Login },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      //{ path: 'clientes/crear', component: CrearCliente },
      // otras rutas...
    ]
  },
  { path: '**', redirectTo: 'login' },
  { path: 'fondos/listar', component: ListarFondos }



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
