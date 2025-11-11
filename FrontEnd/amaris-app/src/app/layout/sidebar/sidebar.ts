import { Component } from '@angular/core';
import { StorageService } from '../../services/core/storage';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  rol: string  = "";

  constructor(private storage: StorageService){
  }

  menuItems = [
    { label: 'Clientes', path: '/clientes', roles: ['Admin', 'User'] },
    { label: 'Fondos', path: '/fondos', roles: ['User'] },
    { label: 'Usuarios', path: '/usuarios', roles: ['Admin'] }
  ];


  ngOnInit(): void {
    this.rol = this.storage.get('rol') || "";
  }

  get visibleItems() {
    return this.menuItems.filter(item => item.roles.includes(this.rol));
  }


}
