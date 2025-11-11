import { Injectable } from '@angular/core';
import { BackendFondosApplicationDTOsUsuarioDto, UsuariosService } from '../fondos-api';
import { Usuario } from '../../model/models';
import { catchError, Observable, throwError } from 'rxjs';
import { ListarUsuarios } from '../../features/usuarios/listar-usuarios/listar-usuarios';

@Injectable({
  providedIn: 'root',
})
export class UsuariosServices {

  constructor(private usuarioService: UsuariosService) {}
  
  listarUsuarios(): Observable<Usuario[]> {
    return this.usuarioService.backendFondosApiEndpointsConsultarUsuariosEndpoint().pipe(
      catchError((error) => {
        console.error('Error al consultar usuarios:', error);
        return throwError(() => new Error('No se pudieron obtener los usuarios.'));
      })
    );
  }


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
