import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ListarCliente } from './listar-cliente/listar-cliente';
import { CrearCliente } from './crear-cliente/crear-cliente';
import { DetalleCliente } from './detalle-cliente/detalle-cliente';
import { HistorialTransacciones } from './historial-transacciones/historial-transacciones';
import { AuthGuard } from '../../services/core/auth-guard';




@NgModule({
  declarations: [ListarCliente, CrearCliente, DetalleCliente, HistorialTransacciones],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: '', component: ListarCliente },
      { path: 'listar-cliente', component: ListarCliente, canActivate: [AuthGuard] },
      { path: 'crear-cliente', component: CrearCliente, canActivate: [AuthGuard] },
      { path: 'detalle-cliente/:id', component: DetalleCliente, canActivate: [AuthGuard],data: {  renderMode: 'server' }},
      { path: 'cliente-transacciones', component: HistorialTransacciones, canActivate: [AuthGuard] }
    ])
  ]
})
export class ClientesModule { }
