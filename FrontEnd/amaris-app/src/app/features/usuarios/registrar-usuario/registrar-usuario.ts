import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsuariosServices } from '../../../services/core/usuarios-services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar-usuario',
  standalone: false,
  templateUrl: './registrar-usuario.html',
  styleUrl: './registrar-usuario.scss',
})
export class RegistrarUsuario {
  formUsuario!: FormGroup;

  constructor(private usuariosService: UsuariosServices, private fb: FormBuilder, private router: Router){}

  ngOnInit(): void {
    this.formUsuario = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      rol: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  registrarUsuario(): void {
    if (this.formUsuario.invalid) return;
  
    const nuevoUsuario = this.formUsuario.value;
    this.usuariosService.registrarUsuario(nuevoUsuario).subscribe({
      next: () => {
        this.router.navigate(['/usuarios']);
      },
      error: err => {
        console.error('Error al registrar usuario:', err);
      }
    });
  }
  
}
