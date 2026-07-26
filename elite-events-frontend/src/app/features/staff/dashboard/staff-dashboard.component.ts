import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-staff-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Staff Dashboard</h1>
      <p>Welcome to the Staff portal. View tasks, check-in attendees, and manage event schedules.</p>
    </div>
  `
})
export class StaffDashboardComponent {}
