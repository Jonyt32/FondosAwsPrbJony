import { Injectable } from '@angular/core';
import * as rxjs from 'rxjs';
import { FondosService } from '../fondos-api/api/fondos.service';
import { BackendFondosApplicationDTOsFondoDto } from '../fondos-api/model/backendFondosApplicationDTOsFondoDto';

  
@Injectable({
  providedIn: 'root',
})
export class FondosServices {

  constructor(private fondosServices: FondosService ) {}

  

  CrearFondo(fondo:any){
    if(!fondo){
      throw new Error('El fondo no puede ser nulo o indefinido');
    }
    let fondoDto: BackendFondosApplicationDTOsFondoDto;
    fondoDto  = {
      fondoID: fondo.fondoID,
      nombreFondo: fondo.nombreFondo,
      montoMinimo: fondo.montoMinimo,
      categoria: fondo.categoria
    }

    try {
      this.fondosServices.crearFondosEndpoint(fondoDto).subscribe({
        next: (response) => {
          return response;
        }
      });
    } catch (error) {
      throw new Error('Error en CrearFondo'+ error);
    }

  }

  ObtenerFondos() {
    try {
       this.fondosServices.consultarFondosEndpoint().subscribe({
        next: (fondos) => {
          return fondos;
        }
       });
    } catch (error) {
      throw  new Error('Error al actualizar el saldo del cliente: ' + error);
    }
  }




}
