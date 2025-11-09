import { Injectable } from '@angular/core';
import * as rxjs from 'rxjs';
import { FondosService } from '../fondos-api/api/fondos.service';
import { BackendFondosApplicationDTOsFondoDto } from '../fondos-api/model/backendFondosApplicationDTOsFondoDto';
import { Fondo } from '../../model/models';
import { catchError, Observable, throwError } from 'rxjs';

  
@Injectable({
  providedIn: 'root',
})
export class FondosServices {

  constructor(private fondosServices: FondosService) {}

  crearFondo(fondo: Fondo): Observable<void> {
    if (!fondo) {
      return throwError(() => new Error('El fondo no puede ser nulo o indefinido'));
    }

    const fondoDto: BackendFondosApplicationDTOsFondoDto = {
      fondoID: fondo.fondoID,
      nombreFondo: fondo.nombreFondo,
      montoMinimo: fondo.montoMinimo,
      categoria: fondo.categoria
    };

    return this.fondosServices.crearFondosEndpoint(fondoDto).pipe(
      catchError((error) => {
        console.error('Error en crearFondo:', error);
        return throwError(() => new Error('Error al crear el fondo.'));
      })
    );
  }

  obtenerFondos(): Observable<BackendFondosApplicationDTOsFondoDto[]> {
    return this.fondosServices.consultarFondosEndpoint().pipe(
      catchError((error) => {
        console.error('Error en obtenerFondos:', error);
        return throwError(() => new Error('Error al consultar los fondos.'));
      })
    );
  }

}
