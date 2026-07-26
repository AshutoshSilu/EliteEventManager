import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-clients-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Client Management</h2><p>Manage clients and event requests.</p></div>`
})
export class ClientsManageComponent {}
