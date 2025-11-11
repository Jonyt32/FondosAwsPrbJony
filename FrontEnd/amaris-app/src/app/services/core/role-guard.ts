import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { StorageService } from './storage';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private storage: StorageService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRoles = route.data['roles'] as string[];
    const userRole = this.storage.get('rol')  || "";

    if (!expectedRoles.includes(userRole)) {
      this.router.navigate(['/unauthorized']); 
      return false;
    }

    return true;
  }
}
