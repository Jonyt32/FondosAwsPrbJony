import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { UsuariosServices } from '../../../services/core/usuarios-services';
import { Router } from '@angular/router';
import { Usuario } from '../../../model/models';

@Component({
  selector: 'app-registrar-usuario',
  standalone: false,
  templateUrl: './registrar-usuario.html',
  styleUrl: './registrar-usuario.scss',
})
export class RegistrarUsuario implements OnInit {
  formUsuario!: FormGroup;

  constructor(
    private usuariosService: UsuariosServices,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.formUsuario = this.fb.group({
      nombreUsuario: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      rol: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: [this.passwordsCoincidenValidator]
    });
  }

  registrarUsuario(): void {
    debugger;
    if (this.formUsuario.invalid) return;

    const nuevoUsuario: Usuario = {
      nombreUsuario: this.formUsuario.value.nombreUsuario,
      email: this.formUsuario.value.email,
      rol: this.formUsuario.value.rol,
      password: this.formUsuario.value.password
    };
    
    this.usuariosService.registrarUsuario(nuevoUsuario).subscribe({
      next: () => this.router.navigate(['/usuarios']),
      error: err => console.error('Error al registrar usuario:', err)
    });
  }

  passwordsCoincidenValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { passwordsNoCoinciden: true };
  }
}