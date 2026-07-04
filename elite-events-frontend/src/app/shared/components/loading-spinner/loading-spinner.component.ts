import { Component, inject } from '@angular/core';
import { LoadingService } from '@core/services/loading.service';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  template: `
    @if (loadingService.isLoading()) {
      <div class="loading-overlay">
        <div class="spinner">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
    }
  `,
  styles: [`
    .loading-overlay {
      position: fixed; top: 0; left: 0; width: 100%; height: 100%;
      background: rgba(0, 0, 0, 0.3); display: flex;
      align-items: center; justify-content: center; z-index: 9999;
    }
    .spinner-border { width: 3rem; height: 3rem; }
  `]
})
export class LoadingSpinnerComponent {
  loadingService = inject(LoadingService);
}
