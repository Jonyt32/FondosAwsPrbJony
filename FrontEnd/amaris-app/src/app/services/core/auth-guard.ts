import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { StorageService } from './storage';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private storage: StorageService) {}

  canActivate(): boolean | UrlTree {
    const token = this.storage.get('token');
    if (!token) {
      return this.router.createUrlTree(['/login']);
    }
    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded?.exp;
      const now = Math.floor(Date.now() / 1000);
      return exp && exp > now ? true : this.router.createUrlTree(['/login']);
    } catch {
      return this.router.createUrlTree(['/login']);
    }
  }
}


