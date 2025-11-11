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
      { path: 'listar-cliente', component: ListarCliente},
      { path: 'crear-cliente', component: CrearCliente },
      { path: 'detalle-cliente/:id', component: DetalleCliente },
      { path: 'cliente-transacciones', component: HistorialTransacciones }
    ])
  ]
})
export class ClientesModule { }
