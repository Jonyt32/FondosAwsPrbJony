import { Component } from '@angular/core';
import { StorageService } from '../../services/core/storage';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  usuario: string | null = null;
  rol: string | null = null;

  constructor(private storage: StorageService, private router:Router) {

  }
  
  ngOnInit(): void {
    this.usuario = this.storage.get('usuario') ?? 'Invitado';
    this.rol = this.storage.get('rol') ?? 'Sin rol';
  }

  logout(): void {
    this.storage.remove('token');
    this.router.navigate(['/login']);
  }


}
