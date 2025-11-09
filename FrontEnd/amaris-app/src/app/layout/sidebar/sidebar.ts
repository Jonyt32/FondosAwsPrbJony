import { Component } from '@angular/core';
import { StorageService } from '../../services/core/storage';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  rol: string | null = null;

  constructor(private storage: StorageService){

  }

  get menuItems(): string[] {
    switch (this.rol) {
      case 'Admin':
        return ['Usuarios', 'Fondos', 'Transacciones'];
      case 'User':
        return ['Mis Fondos', 'Mis Transacciones'];
      default:
        return [];
    }
  }

  ngOnInit(): void {
    this.rol = this.storage.get('rol');
  }

}
