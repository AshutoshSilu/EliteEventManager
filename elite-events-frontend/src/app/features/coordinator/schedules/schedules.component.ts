import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-schedules',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Schedules</h2><p>Manage event schedules.</p></div>`
})
export class SchedulesComponent {}
