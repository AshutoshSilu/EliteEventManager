import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-volunteer-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Volunteer Dashboard</h1>
      <p>Welcome to the Volunteer portal. View shifts, update availability, and manage tasks.</p>
    </div>
  `
})
export class VolunteerDashboardComponent {}
