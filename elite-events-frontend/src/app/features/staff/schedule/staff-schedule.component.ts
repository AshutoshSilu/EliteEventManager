import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-staff-schedule',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Event Schedule</h2><p>Access event schedule information.</p></div>`
})
export class StaffScheduleComponent {}
