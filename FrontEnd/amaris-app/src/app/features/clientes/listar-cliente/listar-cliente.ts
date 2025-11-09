import { Component } from '@angular/core';
import { Cliente } from '../../../model/models';
import { ClientesServices } from '../../../services/core/clientes-services';

@Component({
  selector: 'app-listar-cliente',
  standalone: false,
  templateUrl: './listar-cliente.html',
  styleUrl: './listar-cliente.scss',
})
export class ListarCliente {
  clientes: Cliente[] = [];
  mostrarFormulario = false;

  constructor(private clientesServices: ClientesServices) {}

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    /*this.clientesServices.obtenerClientes().subscribe({
      next: (data) => this.clientes = data,
      error: (err) => console.error('Error al obtener clientes:', err)
    });*/
  }

  toggleFormulario(): void {
    this.mostrarFormulario = !this.mostrarFormulario;
  }

  onClienteCreado(): void {
    this.mostrarFormulario = false;
    this.cargarClientes();
  }

}
