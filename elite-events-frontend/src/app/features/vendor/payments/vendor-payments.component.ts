import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-payments',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Payments</h2><p>View payment status.</p></div>`
})
export class VendorPaymentsComponent {}
