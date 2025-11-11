import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarUsuario } from './registrar-usuario/registrar-usuario';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthGuard } from '../../services/core/auth-guard';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    RegistrarUsuario
  ],
  imports:[
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: '', component: RegistrarUsuario },
      { path: 'registrar-usuario', component: RegistrarUsuario, canActivate: [AuthGuard] }
    ])
  ]
})
export class UsuariosModule { }

