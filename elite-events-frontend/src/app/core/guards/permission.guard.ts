import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';
import { AuthService } from '../services/auth.service';
import { Permission } from '../constants/permissions';

/**
 * Permission-based route guard.
 * Checks if user has the required permissions defined in route data.
 *
 * Usage in route config:
 * {
 *   path: 'users',
 *   canActivate: [permissionGuard],
 *   data: { permissions: ['users.view'] }           // requires ALL listed permissions
 * }
 *
 * Or for ANY-match mode:
 * {
 *   path: 'reports',
 *   canActivate: [permissionGuard],
 *   data: { permissions: ['reports.view', 'analytics.view'], permissionMode: 'any' }
 * }
 */
export const permissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const permissionService = inject(PermissionService);
  const authService = inject(AuthService);
  const router = inject(Router);

  // If user is not logged in, redirect to login
  if (!authService.isLoggedIn()) {
    router.navigate(['/auth/login'], { queryParams: { returnUrl: window.location.pathname } });
    return false;
  }

  const requiredPermissions = route.data['permissions'] as Permission[] | undefined;

  // No permissions required — allow access
  if (!requiredPermissions || requiredPermissions.length === 0) {
    return true;
  }

  const mode = (route.data['permissionMode'] as string) ?? 'all';

  const hasAccess = mode === 'any'
    ? permissionService.hasAnyPermission(requiredPermissions)
    : permissionService.hasAllPermissions(requiredPermissions);

  if (hasAccess) {
    return true;
  }

  // Redirect to 403 unauthorized page
  router.navigate(['/unauthorized']);
  return false;
};
