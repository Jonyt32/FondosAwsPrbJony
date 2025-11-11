import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarUsuario } from './registrar-usuario/registrar-usuario';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthGuard } from '../../services/core/auth-guard';
import { RouterModule } from '@angular/router';
import { ListarUsuarios } from './listar-usuarios/listar-usuarios';


@NgModule({
  declarations: [
    RegistrarUsuario,
    ListarUsuarios
  ],
  imports:[
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: '', component: ListarUsuarios },
      { path: 'listar-usuario', component: ListarUsuarios },
      { path: 'registrar-usuario', component: RegistrarUsuario }
    ])
  ]
})
export class UsuariosModule { }

