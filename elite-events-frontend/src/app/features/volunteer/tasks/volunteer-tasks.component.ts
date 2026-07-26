import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-volunteer-tasks',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>My Tasks</h2><p>Mark tasks as completed.</p></div>`
})
export class VolunteerTasksComponent {}
