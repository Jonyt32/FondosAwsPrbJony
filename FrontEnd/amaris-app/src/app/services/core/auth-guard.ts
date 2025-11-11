import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { StorageService } from './storage';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private storage:StorageService) {}

  canActivate(): boolean {
    const token = this.storage.get('token');
    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }
    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded.exp;
      const now = Math.floor(Date.now() / 1000);
      
      if (exp && exp > now) {
        return true;
      } else {
        console.warn('Token expirado');
        this.router.navigate(['/login']);
        return false;
      }
    } catch (err) {
      console.error('Token inválido');
      this.router.navigate(['/login']);
      return false;
    }

  }
}


