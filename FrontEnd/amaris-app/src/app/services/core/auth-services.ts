import { Injectable } from '@angular/core';
import * as rxjs from 'rxjs';
import { AuthService } from '../../services/fondos-api/api/auth.service';
import { AuthRequest, AuthResponse } from '../fondos-api';

@Injectable({
  providedIn: 'root',
})
export class AuthServices {
  
  constructor(private authServices: AuthService)
  {

  }

  login(credentials: AuthRequest): rxjs.Observable<AuthResponse> {
      try {
        return this.authServices.backendFondosApiEndpointsLoginEndpoint(credentials);
      } catch (error) {
        throw new Error('Error en el servicio de login: ' + error);
      }
      
    }

}
