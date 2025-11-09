import { Component, Input } from '@angular/core';
import { Transaccion } from '../../../model/models';
import { ClientesServices } from '../../../services/core/clientes-services';

@Component({
  selector: 'app-historial-transacciones',
  standalone: false,
  templateUrl: './historial-transacciones.html',
  styleUrl: './historial-transacciones.scss',
})
export class HistorialTransacciones {
  @Input() clienteId: string = '';
  transacciones: Transaccion[] = [];

  constructor(private transaccionesServices: ClientesServices) {}

  ngOnInit(): void {
    if (this.clienteId) {
      this.transaccionesServices.obtenerTransaccionesCliente(this.clienteId).subscribe({
        next: (data) => this.transacciones = data,
        error: (err) => console.error(err.message)
      });
    }
  }

}
