import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ListarCliente } from './listar-cliente/listar-cliente';
import { CrearCliente } from './crear-cliente/crear-cliente';
import { DetalleCliente } from './detalle-cliente/detalle-cliente';
import { HistorialTransacciones } from './historial-transacciones/historial-transacciones';




@NgModule({
  declarations: [ListarCliente, CrearCliente, DetalleCliente, HistorialTransacciones],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule
  ]
})
export class ClientesModule { }
