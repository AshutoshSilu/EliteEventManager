import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-events',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>My Events</h2><p>View your assigned events.</p></div>`
})
export class VendorEventsComponent {}
