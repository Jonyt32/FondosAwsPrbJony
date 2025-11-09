import { Component, Input } from '@angular/core';
import { Cliente, Fondo } from '../../../model/models';
import { FondosServices } from '../../../services/core/fondos-services';

@Component({
  selector: 'app-detalle-cliente',
  standalone: false,
  templateUrl: './detalle-cliente.html',
  styleUrl: './detalle-cliente.scss',
})
export class DetalleCliente {
  @Input() cliente: Cliente = {};
  fondos: Fondo[] = [];
  mostrarHistorial = false;
  
  constructor(private fondosServices: FondosServices) {}

  ngOnInit(): void {
    this.cargarFondos();
  }

  cargarFondos(): void {
    this.fondosServices.obtenerFondos().subscribe({
      next: (data) => this.fondos = data,
      error: (err) => console.error('Error al obtener fondos:', err)
    });
  }

  estaSuscrito(fondoID: string): boolean {
    return this.cliente.fondosActivos?.includes(fondoID) ?? false;
  }

  toggleSuscripcion(fondoID: string): void {
    const suscrito = this.estaSuscrito(fondoID);

    if (suscrito) {
      this.cliente.fondosActivos = this.cliente.fondosActivos?.filter(id => id !== fondoID);
    } else {
      this.cliente.fondosActivos = [...(this.cliente.fondosActivos ?? []), fondoID];
    }

    // Aquí podrías llamar a un servicio para persistir el cambio
    console.log(`Cliente actualizado:`, this.cliente.fondosActivos);
  }


}
