import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-registrations-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Registrations</h2><p>Manage event registrations.</p></div>`
})
export class RegistrationsManageComponent {}
