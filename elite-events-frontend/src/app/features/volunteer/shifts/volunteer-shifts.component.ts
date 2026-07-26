import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-volunteer-shifts',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>My Shifts</h2><p>View your assigned shifts.</p></div>`
})
export class VolunteerShiftsComponent {}
