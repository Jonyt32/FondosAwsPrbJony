import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { LoginServices } from '../../services/core/login-services';
import { AuthRequest, AuthResponse } from '../../services/fondos-api';
import { Router } from '@angular/router';
import { StorageService } from '../../services/core/storage';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  loginForm: FormGroup;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authServices: LoginServices,
    private router: Router,
    private storage: StorageService     
  ) {
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required, Validators.email]],
      contrasena: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    const credentials: AuthRequest = {
      username: this.loginForm.value.correoElectronico,
      password: this.loginForm.value.contrasena
    };

    this.authServices.login(credentials).subscribe({
      next: (response: AuthResponse) => {
        const rol = Array.isArray(response.roles) && response.roles.length > 0 ? response.roles[0] : '';
        this.storage.set('token', response.token || "");
        this.storage.set('rol', rol);
        this.storage.set('usuario', response.userId || "");
        
        this.router.navigate(['/dashboard']); // ajusta según tu ruta principal
      },
      error: (err) => {
        this.errorMessage = err.message;
        console.error('Login error:', err);
      }
    });
  }

}
