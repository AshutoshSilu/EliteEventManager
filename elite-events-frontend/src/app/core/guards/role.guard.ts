import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';
import { AuthService } from '../services/auth.service';
import { Role } from '../constants/permissions';

/**
 * Role-based access guard.
 * Checks if user has any of the required roles defined in route data.
 *
 * Usage in route config:
 * {
 *   path: 'admin',
 *   canActivate: [authGuard, roleGuard],
 *   data: { roles: ['SuperAdmin', 'Admin', 'EventManager'] }
 * }
 */
export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const permissionService = inject(PermissionService);
  const authService = inject(AuthService);
  const router = inject(Router);

  // If user is not logged in, redirect to login
  if (!authService.isLoggedIn()) {
    router.navigate(['/auth/login'], { queryParams: { returnUrl: window.location.pathname } });
    return false;
  }

  const requiredRoles = route.data['roles'] as Role[] | undefined;

  // No roles required — allow access
  if (!requiredRoles || requiredRoles.length === 0) {
    return true;
  }

  if (permissionService.hasAnyRole(requiredRoles)) {
    return true;
  }

  // Redirect to 403 unauthorized page
  router.navigate(['/unauthorized']);
  return false;
};
