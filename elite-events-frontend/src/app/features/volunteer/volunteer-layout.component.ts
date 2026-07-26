import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { VOLUNTEER_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-volunteer-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Volunteer Portal"
      brandPrefix="Elite"
      brandAccent="Volunteer">
    </app-role-layout>
  `
})
export class VolunteerLayoutComponent {
  menuItems = VOLUNTEER_SIDEBAR_MENU;
}
