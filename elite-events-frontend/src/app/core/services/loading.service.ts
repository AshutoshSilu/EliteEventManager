import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private loadingCount = 0;
  private isLoadingSignal = signal<boolean>(false);

  readonly isLoading = this.isLoadingSignal.asReadonly();

  show(): void {
    this.loadingCount++;
    this.isLoadingSignal.set(true);
  }

  hide(): void {
    this.loadingCount = Math.max(0, this.loadingCount - 1);
    if (this.loadingCount === 0) {
      this.isLoadingSignal.set(false);
    }
  }
}
