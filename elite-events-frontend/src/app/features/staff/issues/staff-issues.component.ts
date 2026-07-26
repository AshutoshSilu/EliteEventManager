import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-staff-issues',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Report Issues</h2><p>Report issues encountered during events.</p></div>`
})
export class StaffIssuesComponent {}
