import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-roles-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Role Management</h2><p>Manage roles and permissions.</p></div>`
})
export class RolesManageComponent {}
