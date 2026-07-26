import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Vendor Dashboard</h1>
      <p>Welcome to the Vendor portal. Manage your products, bookings, and invoices.</p>
    </div>
  `
})
export class VendorDashboardComponent {}
