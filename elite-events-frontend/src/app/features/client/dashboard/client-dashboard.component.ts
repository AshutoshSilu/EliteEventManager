import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Client Dashboard</h1>
      <p>Welcome to the Client portal. Create event requests, track progress, and manage communications.</p>
    </div>
  `
})
export class ClientDashboardComponent {}
