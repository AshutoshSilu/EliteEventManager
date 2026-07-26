import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { CLIENT_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-client-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Client Portal"
      brandPrefix="Elite"
      brandAccent="Client">
    </app-role-layout>
  `
})
export class ClientLayoutComponent {
  menuItems = CLIENT_SIDEBAR_MENU;
}
