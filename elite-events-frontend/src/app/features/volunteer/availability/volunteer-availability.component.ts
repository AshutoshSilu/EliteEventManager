import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-volunteer-availability',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Availability</h2><p>Update your availability schedule.</p></div>`
})
export class VolunteerAvailabilityComponent {}
