import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Analytics</h2><p>View event and business analytics.</p></div>`
})
export class AnalyticsComponent {}
