import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { LoadingSpinnerComponent } from '@shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, LoadingSpinnerComponent],
  template: `
    <app-loading-spinner />
    <app-header />
    <main class="main-content">
      <router-outlet />
    </main>
    <app-footer />
  `,
  styles: [`.main-content { min-height: calc(100vh - 200px); }`]
})
export class PublicLayoutComponent {}
