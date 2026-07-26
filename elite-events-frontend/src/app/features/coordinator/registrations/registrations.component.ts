import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-registrations',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Registrations</h2><p>Manage attendee registrations.</p></div>`
})
export class RegistrationsComponent {}
