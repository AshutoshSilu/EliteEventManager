import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-checkin',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Check-In</h2><p>Check in attendees for events.</p></div>`
})
export class CheckinComponent {}
