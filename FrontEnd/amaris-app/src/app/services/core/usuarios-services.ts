import { Injectable } from '@angular/core';
import { BackendFondosApplicationDTOsUsuarioDto, UsuariosService } from '../fondos-api';

@Injectable({
  providedIn: 'root',
})
export class UsuariosServices {

  constructor(private usuarioService: UsuariosService){
    
  }

  RegistrarUsuarios(usuario: any ) {

    let usuarioDto: BackendFondosApplicationDTOsUsuarioDto;
    if(!usuario)
      throw new Error('El objeto usuario es nulo o indefinido.');

    try {
      usuarioDto = { 
        nombreUsuario: usuario.nombreUsuario,
        email: usuario.email,
        rol: usuario.rol,
        password: usuario.password
      }  

      return this.usuarioService.backendFondosApiEndpointsRegistrarUsuarioEndpoint(usuarioDto);

    } catch (error) {
      throw new Error('Error al mapear los datos del usuario: ' + error);
    }
  }
}
