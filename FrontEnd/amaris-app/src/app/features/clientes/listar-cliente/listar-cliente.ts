import { Component } from '@angular/core';
import { Cliente } from '../../../model/models';
import { ClientesServices } from '../../../services/core/clientes-services';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-listar-cliente',
  standalone: false,
  templateUrl: './listar-cliente.html',
  styleUrl: './listar-cliente.scss',
})
export class ListarCliente {
  clientes: Cliente[] = [];
  mostrarFormulario = false;
  filtroCorreo: string = '';
  constructor(private clientesServices: ClientesServices, private router: Router) {}

  ngOnInit(): void {
    this.cargarClientes();
    
  }

  cargarClientes(): void {
    this.clientesServices.filtrarClientesPorCorreo(this.filtroCorreo).subscribe({
      next: (data) => {
        this.clientes = data} ,
      error: (err) => console.error('Error al obtener clientes:', err)
    });
  }
  
  crearCliente(){
    this.router.navigate(['/clientes/crear-cliente']);
  }

  consultarCliente(clienteId: string){
    this.router.navigate(['/clientes/detalle-cliente', clienteId]);
  }


  toggleFormulario(): void {
    this.mostrarFormulario = !this.mostrarFormulario;
  }

  onClienteCreado(): void {
    this.mostrarFormulario = false;
    this.cargarClientes();
  }
}
