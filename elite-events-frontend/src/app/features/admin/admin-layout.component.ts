import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { ADMIN_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Admin Panel"
      brandPrefix="Elite"
      brandAccent="Admin">
    </app-role-layout>
  `
})
export class AdminLayoutComponent {
  menuItems = ADMIN_SIDEBAR_MENU;
}
