import { Injectable } from '@angular/core';
import { BackendFondosApplicationDTOsClienteDto, BackendFondosApplicationDTOsFondoDto, BackendFondosApplicationDTOsResultadoOperacionDto, BackendFondosApplicationDTOsTransaccionDto, BackendFondosLambdasCancelarSuscripcionCancelacionRequestDto, ClientesService } from '../fondos-api';
import { Cliente, Fondo, Transaccion } from '../../model/models';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ClientesServices {
  constructor( private clienteService: ClientesService) {}
  
  subscribirCliente(clientId: string, fondoId: string): Observable<BackendFondosApplicationDTOsResultadoOperacionDto> {
    return this.clienteService.subscribirClienteAFondoEndpoint(clientId, fondoId).pipe(
      map((response) => {
        if (!response.exito) {
          throw new Error(`Error al suscribir el fondo al cliente: ${response.mensajeNotificacion}`);
        }
        return response;
      }),
      catchError((error) => {
        console.error('Error en SubscribirCliente:', error);
        return throwError(() => new Error('Error al suscribir el fondo al cliente.'));
      })
    );
  }

  actualizarSaldoCliente(clientId: string, nuevoSaldo: number): Observable<void> {
    return this.clienteService.actualizarSaldoClienteEndpoint(clientId, nuevoSaldo.toString()).pipe(
      catchError((error) => {
        console.error('Error en ActualizarSaldoCliente:', error);
        return throwError(() => new Error('Error al actualizar el saldo del cliente.'));
      })
    );
  }

  obtenerFondosCliente(clientId: string): Observable<Fondo[]> {
    return this.clienteService.fondosAsignadosClienteEndpoint(clientId).pipe(
      map((fondosDto: BackendFondosApplicationDTOsFondoDto[]) =>
        fondosDto.map((dto) => ({
          fondoID: dto.fondoID,
          nombreFondo: dto.nombreFondo,
          montoMinimo: dto.montoMinimo,
          categoria: dto.categoria,
        }))
      ),
      catchError((error) => {
        console.error('Error en FondosCliente:', error);
        return throwError(() => new Error('Error al obtener los fondos del cliente.'));
      })
    );
  }

  cancelarSuscripcion(clientId: string, fondoId: string): Observable<void> {
    const cancelarDto: BackendFondosLambdasCancelarSuscripcionCancelacionRequestDto = {
      clienteId: clientId,
      fondoId: fondoId
    };

    return this.clienteService.cancelarSuscripcionEndpoint(cancelarDto).pipe(
      catchError((error) => {
        console.error('Error en cancelarSuscripcion:', error);
        return throwError(() => new Error('Error al cancelar la suscripción.'));
      })
    );
  }

  crearCliente(cliente: Cliente): Observable<void> {
    const clientDto: BackendFondosApplicationDTOsClienteDto = {
      nombre: cliente.nombre,
      preferenciaNotificacion: cliente.preferenciaNotificacion,
      saldoDisponible: 1000 // valor inicial fijo
    };

    return this.clienteService.crearClienteEndpoint(clientDto).pipe(
      catchError((error) => {
        console.error('Error en crearCliente:', error);
        return throwError(() => new Error('Error al crear el cliente.'));
      })
    );
  }

  obtenerTransaccionesCliente(clientId: string): Observable<Transaccion[]> {
    return this.clienteService.consultarTransaccionesEndpoint(clientId).pipe(
      map((dtoList: BackendFondosApplicationDTOsTransaccionDto[]) =>
        dtoList.map((dto) => ({
          tipo: dto.tipo,
          fondoID: dto.fondoID,
          monto: dto.monto,
          fecha: dto.fecha,
          notificacion: dto.notificacion
        }))
      ),
      catchError((error) => {
        console.error('Error en obtenerTransaccionesCliente:', error);
        return throwError(() => new Error('Error al obtener las transacciones del cliente.'));
      })
    );
  }


}
