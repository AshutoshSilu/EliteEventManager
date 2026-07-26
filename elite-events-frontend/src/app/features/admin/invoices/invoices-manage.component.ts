import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invoices-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Invoices</h2><p>Manage invoices and billing.</p></div>`
})
export class InvoicesManageComponent {}
