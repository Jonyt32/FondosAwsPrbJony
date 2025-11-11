import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { LoginServices } from '../../services/core/login-services';
import { AuthRequest, AuthResponse } from '../../services/fondos-api';
import { Router } from '@angular/router';
import { StorageService } from '../../services/core/storage';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  loginForm: FormGroup = new FormGroup({});
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authServices: LoginServices,
    private router: Router,
    private storage: StorageService     
  ) {
    
  }

  ngOnInit(): void {
    
    const token = localStorage.getItem('token');
    const currentUrl = this.router.url;

    if (currentUrl === '/login' && token && token.includes('.')) {
      try {
        const decoded: any = jwtDecode(token);
        const now = Math.floor(Date.now() / 1000);
        if (decoded.exp && decoded.exp > now) {
          this.router.navigate(['/clientes']); // o a donde quieras redirigir
        }
      } catch {
        // token inválido, no redirigir
      }
    }


    this.loginForm = this.fb.group({
      txtCorreo: ['', [Validators.required, Validators.pattern('[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$')]],
      txtPassword: ['', Validators.required]
    });
  }

  get correoNoValido(){
    const control = this.loginForm.get('txtCorreo')
    return control?.invalid && control?.touched
  }

  get pass1NoValido(){
    const control = this.loginForm.get('txtPassword')
    return control?.invalid && control?.touched
  }

  login(): void {
  
    this.loginForm.markAllAsTouched();
    if (this.loginForm.invalid) return;

    const credentials: AuthRequest = {
      username: this.loginForm.value.txtCorreo,
      password: this.loginForm.value.txtPassword
    };
   
    this.authServices.login(credentials).subscribe({
      next: (response: AuthResponse) => {
        const rol = Array.isArray(response.roles) && response.roles.length > 0 ? response.roles[0] : '';
        this.storage.set('token', response.token || "");
        this.storage.set('rol', rol);
        this.storage.set('usuario', response.userId || "");
        
        this.router.navigate(['/clientes']); // ajusta según tu ruta principal
      },
      error: (err) => {
        
        this.errorMessage = err.message;
        console.error('Login error:', err);
      }
    });
  }

}


