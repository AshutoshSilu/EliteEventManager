import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-volunteers-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Volunteer Management</h2><p>Manage volunteers and assignments.</p></div>`
})
export class VolunteersManageComponent {}
