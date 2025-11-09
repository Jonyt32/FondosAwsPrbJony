import { Injectable } from '@angular/core';
import * as rxjs from 'rxjs';
import { AuthService } from '../fondos-api/api/auth.service';
import { AuthRequest, AuthResponse } from '../fondos-api';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoginServices {
  
  constructor(private authServices: AuthService) {}

  login(credentials: AuthRequest): rxjs.Observable<AuthResponse> {
    return this.authServices.backendFondosApiEndpointsLoginEndpoint(credentials).pipe(
      catchError((error) => {
        console.error('Error en AuthServices.login:', error);
        return throwError(() => new Error('Error en el servicio de login.'));
      })
    );
  }
}
