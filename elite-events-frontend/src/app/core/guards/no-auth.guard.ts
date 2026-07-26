import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Prevents authenticated users from accessing login/register pages.
 * Redirects to the user's role-appropriate dashboard.
 */
export const noAuthGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isLoggedIn()) {
    return true;
  }

  // Redirect authenticated user to their default dashboard
  const dashboard = authService.getDefaultDashboard();
  router.navigate([dashboard]);
  return false;
};
