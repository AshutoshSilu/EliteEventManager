import { Component, Input, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { PermissionService } from '@core/services/permission.service';
import { NotificationService } from '@core/services/notification.service';
import { SidebarMenuItem } from '@core/constants/sidebar-menu';
import { Permission } from '@core/constants/permissions';

/**
 * Reusable layout component with dynamic sidebar.
 * Filters menu items based on user permissions.
 * Used by all role-specific dashboards (Admin, Coordinator, Vendor, Staff, Volunteer, Client).
 */
@Component({
  selector: 'app-role-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './role-layout.component.html',
  styleUrls: ['./role-layout.component.scss']
})
export class RoleLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private permissionService = inject(PermissionService);
  private notificationService = inject(NotificationService);

  /** Menu items to display (filtered by permissions at runtime) */
  @Input() menuItems: SidebarMenuItem[] = [];

  /** Panel title shown in sidebar brand area */
  @Input() panelTitle = 'Dashboard';

  /** Accent text in brand */
  @Input() brandAccent = 'Events';

  /** Brand prefix text */
  @Input() brandPrefix = 'Elite';

  user = this.authService.user;
  unreadCount = signal(0);
  sidebarOpen = false;
  filteredMenu: SidebarMenuItem[] = [];

  ngOnInit(): void {
    this.filteredMenu = this.filterMenuItems(this.menuItems);
    this.notificationService.getUnreadCount().subscribe(res => {
      if (res.success && res.data !== undefined) this.unreadCount.set(res.data);
    });
  }

  logout(): void {
    this.authService.logout();
  }

  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  closeSidebar(): void {
    this.sidebarOpen = false;
  }

  /**
   * Filter menu items based on user's current permissions.
   * Items without permissions are always shown (to authenticated users).
   */
  private filterMenuItems(items: SidebarMenuItem[]): SidebarMenuItem[] {
    return items.filter(item => {
      // No permission requirement — always visible
      if (!item.permissions || item.permissions.length === 0) {
        return true;
      }

      // Check role restriction first
      if (item.roles && item.roles.length > 0) {
        if (!this.permissionService.hasAnyRole(item.roles)) {
          return false;
        }
      }

      // Check permissions
      const mode = item.permissionMode ?? 'any';
      if (mode === 'all') {
        return this.permissionService.hasAllPermissions(item.permissions as Permission[]);
      }
      return this.permissionService.hasAnyPermission(item.permissions as Permission[]);
    });
  }
}
