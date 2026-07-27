import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

/**
 * Global HTTP error interceptor with toast notifications.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);
  const authService = inject(AuthService);
  const isAuthEndpoint = req.url.includes('/auth/');

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      switch (error.status) {
        case 0:
          errorMessage = 'Unable to connect to server. Please check your internet connection.';
          break;
        case 400:
          errorMessage = error.error?.message || 'Invalid request. Please check your input.';
          if (error.error?.errors?.length) {
            errorMessage = error.error.errors.join('\n');
          }
          break;
        case 401:
          if (isAuthEndpoint) {
            errorMessage = error.error?.message || 'Invalid email or password.';
          } else if (authService.isLoggedIn()) {
            errorMessage = 'Your session has expired. Please login again.';
            authService.logout();
          } else {
            errorMessage = error.error?.message || 'Unauthorized request.';
          }
          break;
        case 403:
          errorMessage = 'You do not have permission to perform this action.';
          break;
        case 404:
          errorMessage = error.error?.message || 'The requested resource was not found.';
          break;
        case 409:
          errorMessage = error.error?.message || 'A conflict occurred.';
          break;
        case 500:
          errorMessage = 'An internal server error occurred. Please try again later.';
          break;
      }

      // Do not show noise toast when logout call itself fails while clearing session.
      if (!req.url.includes('/auth/logout')) {
        toastr.error(errorMessage, 'Error');
      }
      return throwError(() => error);
    })
  );
};
