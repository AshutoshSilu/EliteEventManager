import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="unauthorized-container">
      <div class="unauthorized-card">
        <div class="icon-wrapper">
          <span class="material-icons error-icon">lock</span>
        </div>
        <h1>403</h1>
        <h2>Access Denied</h2>
        <p>You do not have permission to access this page. If you believe this is an error, please contact your administrator.</p>
        <div class="actions">
          <a [routerLink]="getDashboardRoute()" class="btn btn-primary">
            <span class="material-icons">home</span> Go to Dashboard
          </a>
          <button class="btn btn-outline" (click)="goBack()">
            <span class="material-icons">arrow_back</span> Go Back
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .unauthorized-container {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
      padding: 1rem;
    }

    .unauthorized-card {
      text-align: center;
      background: #fff;
      border-radius: 16px;
      padding: 3rem 2.5rem;
      box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
      max-width: 480px;
      width: 100%;
    }

    .icon-wrapper {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 80px;
      height: 80px;
      border-radius: 50%;
      background: #fee2e2;
      margin-bottom: 1.5rem;
    }

    .error-icon {
      font-size: 40px;
      color: #dc2626;
    }

    h1 {
      font-size: 4rem;
      font-weight: 700;
      color: #1e293b;
      margin: 0;
      line-height: 1;
    }

    h2 {
      font-size: 1.5rem;
      font-weight: 600;
      color: #475569;
      margin: 0.5rem 0 1rem;
    }

    p {
      color: #64748b;
      font-size: 1rem;
      line-height: 1.6;
      margin-bottom: 2rem;
    }

    .actions {
      display: flex;
      gap: 1rem;
      justify-content: center;
      flex-wrap: wrap;
    }

    .btn {
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem 1.5rem;
      border-radius: 8px;
      font-size: 0.875rem;
      font-weight: 500;
      text-decoration: none;
      cursor: pointer;
      border: none;
      transition: all 0.2s;
    }

    .btn .material-icons {
      font-size: 18px;
    }

    .btn-primary {
      background: #4f46e5;
      color: #fff;
    }

    .btn-primary:hover {
      background: #4338ca;
    }

    .btn-outline {
      background: transparent;
      color: #4f46e5;
      border: 1px solid #e2e8f0;
    }

    .btn-outline:hover {
      background: #f8fafc;
      border-color: #4f46e5;
    }
  `]
})
export class UnauthorizedComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  getDashboardRoute(): string {
    return this.authService.isLoggedIn()
      ? this.authService.getDefaultDashboard()
      : '/';
  }

  goBack(): void {
    window.history.back();
  }
}
