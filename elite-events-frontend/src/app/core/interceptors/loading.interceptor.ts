import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

/**
 * Shows/hides loading spinner for HTTP requests.
 */
export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  // Skip loading for background requests
  if (req.headers.has('X-Skip-Loading')) {
    return next(req);
  }

  loadingService.show();

  return next(req).pipe(
    finalize(() => loadingService.hide())
  );
};
