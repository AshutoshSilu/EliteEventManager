import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Prevents authenticated users from accessing login/register pages.
 */
export const noAuthGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isLoggedIn()) {
    return true;
  }

  // Redirect based on role
  const role = authService.userRole();
  if (role === 'Administrator' || role === 'EventManager') {
    router.navigate(['/admin']);
  } else {
    router.navigate(['/customer']);
  }
  return false;
};
