import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-invoices',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Invoices</h2><p>Upload and manage invoices.</p></div>`
})
export class VendorInvoicesComponent {}
