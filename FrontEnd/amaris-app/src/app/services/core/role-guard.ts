import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { StorageService } from './storage';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate  {
  constructor(private storage: StorageService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const expectedRoles = (route.data['roles'] as string[] | undefined) ?? [];
    const token = this.storage.get('token'); // lectura síncrona
    const userRole = (this.storage.get('rol') || '').toString().trim();

    // Sin token → login
    if (!token) {
      return this.router.createUrlTree(['/login']);
    }

    if (expectedRoles.length > 0 && !expectedRoles.includes(userRole)) {
      return this.router.createUrlTree(['/unauthorized']);
    }
    return true;
  }
}
