import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { STAFF_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-staff-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Staff Portal"
      brandPrefix="Elite"
      brandAccent="Staff">
    </app-role-layout>
  `
})
export class StaffLayoutComponent {
  menuItems = STAFF_SIDEBAR_MENU;
}
