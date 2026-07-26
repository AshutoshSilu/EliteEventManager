import { Component } from '@angular/core';
import { RoleLayoutComponent } from '@shared/components/role-layout/role-layout.component';
import { VENDOR_SIDEBAR_MENU } from '@core/constants/sidebar-menu';

@Component({
  selector: 'app-vendor-layout',
  standalone: true,
  imports: [RoleLayoutComponent],
  template: `
    <app-role-layout
      [menuItems]="menuItems"
      panelTitle="Vendor Portal"
      brandPrefix="Elite"
      brandAccent="Vendor">
    </app-role-layout>
  `
})
export class VendorLayoutComponent {
  menuItems = VENDOR_SIDEBAR_MENU;
}
