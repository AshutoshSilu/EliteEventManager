import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tickets-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Ticket Management</h2><p>Manage tickets and allocations.</p></div>`
})
export class TicketsManageComponent {}
