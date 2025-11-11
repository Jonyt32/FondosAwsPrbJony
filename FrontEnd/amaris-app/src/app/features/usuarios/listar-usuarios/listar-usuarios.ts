import { Component } from '@angular/core';
import { Usuario } from '../../../model/models';
import { UsuariosServices } from '../../../services/core/usuarios-services';

@Component({
  selector: 'app-listar-usuarios',
  standalone: false,
  templateUrl: './listar-usuarios.html',
  styleUrl: './listar-usuarios.scss',
})
export class ListarUsuarios {
  usuarios: Usuario[] = [];

  constructor(private usuariosService: UsuariosServices) {}

  ngOnInit(): void {
    this.usuariosService.listarUsuarios().subscribe({
      next: (data) => this.usuarios = data,
      error: (err) => console.error('Error al cargar usuarios:', err)
    });
  }


}
