import { Injectable } from '@angular/core';
import { BackendFondosApplicationDTOsUsuarioDto, UsuariosService } from '../fondos-api';
import { Usuario } from '../../model/models';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsuariosServices {

  constructor(private usuarioService: UsuariosService) {}

  registrarUsuario(usuario: Usuario): Observable<void> {
    if (!usuario) {
      return throwError(() => new Error('El objeto usuario es nulo o indefinido.'));
    }

    const usuarioDto: BackendFondosApplicationDTOsUsuarioDto = {
      nombreUsuario: usuario.nombreUsuario,
      email: usuario.email,
      rol: usuario.rol,
      password: usuario.password
    };

    return this.usuarioService.backendFondosApiEndpointsRegistrarUsuarioEndpoint(usuarioDto).pipe(
      catchError((error) => {
        console.error('Error en registrarUsuario:', error);
        return throwError(() => new Error('Error al registrar el usuario.'));
      })
    );
  }

}
