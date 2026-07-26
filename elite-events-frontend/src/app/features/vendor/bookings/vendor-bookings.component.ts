import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-bookings',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Bookings</h2><p>Accept or reject bookings.</p></div>`
})
export class VendorBookingsComponent {}
