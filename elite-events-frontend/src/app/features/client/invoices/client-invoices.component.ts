import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-invoices',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Invoices</h2><p>View and download invoices.</p></div>`
})
export class ClientInvoicesComponent {}
