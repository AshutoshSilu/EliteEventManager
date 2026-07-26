import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Coordinator Dashboard</h1>
      <p>Welcome to the Event Coordinator portal. Manage registrations, check-ins, and volunteer assignments.</p>
    </div>
  `
})
export class CoordinatorDashboardComponent {}
