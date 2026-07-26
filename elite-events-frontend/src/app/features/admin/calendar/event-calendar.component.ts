import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-event-calendar',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Event Calendar</h2><p>View events in calendar format.</p></div>`
})
export class EventCalendarComponent {}
