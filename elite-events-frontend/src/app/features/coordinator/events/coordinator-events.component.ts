import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-events',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Events</h2><p>View assigned events.</p></div>`
})
export class CoordinatorEventsComponent {}
