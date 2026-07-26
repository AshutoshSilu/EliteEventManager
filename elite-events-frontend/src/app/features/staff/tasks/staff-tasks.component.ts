import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-staff-tasks',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>My Tasks</h2><p>View and update your assigned tasks.</p></div>`
})
export class StaffTasksComponent {}
