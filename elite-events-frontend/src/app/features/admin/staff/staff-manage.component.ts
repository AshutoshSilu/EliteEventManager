import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-staff-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Staff Management</h2><p>Manage staff members and assignments.</p></div>`
})
export class StaffManageComponent {}
