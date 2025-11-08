import { Injectable } from '@angular/core';
import { ClientesService } from '../fondos-api';

@Injectable({
  providedIn: 'root',
})
export class ClientesServices {
  constructor( private clienteService: ClientesService) {}
  
    
  
   
  
    ActualizarSaldoCliente(clientId: string, nuevoSaldo: number) {
      try {
        this.clienteService.actualizarSaldoClienteEndpoint(clientId, nuevoSaldo.toString());   
      } catch (error) {
        throw  new Error('Error al actualizar el saldo del cliente: ' + error);
      }
    }
}
