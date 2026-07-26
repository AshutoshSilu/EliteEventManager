import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { COORDINATOR_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-coordinator-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Coordinator Panel"
      brandPrefix="Elite"
      brandAccent="Coordinator">
    </app-role-layout>
  `
})
export class CoordinatorLayoutComponent {
  menuItems = COORDINATOR_SIDEBAR_MENU;
}
