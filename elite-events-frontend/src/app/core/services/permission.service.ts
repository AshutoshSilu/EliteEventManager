import { Injectable, signal, computed } from '@angular/core';
import { Permission, Role, ROLE_PERMISSIONS, ROLES } from '../constants/permissions';

/**
 * Centralized permission management service.
 * Loads permissions after login and provides reactive permission checks.
 * Supports both static role-based mapping and dynamic server-provided permissions.
 */
@Injectable({ providedIn: 'root' })
export class PermissionService {
  /** Current user's active permissions */
  private userPermissions = signal<Permission[]>([]);

  /** Current user's roles */
  private userRoles = signal<Role[]>([]);

  /** Reactive read-only access to permissions */
  readonly permissions = this.userPermissions.asReadonly();

  /** Reactive read-only access to roles */
  readonly roles = this.userRoles.asReadonly();

  /** Whether any permissions are loaded (user is authenticated with RBAC data) */
  readonly isLoaded = computed(() => this.userPermissions().length > 0 || this.userRoles().length > 0);

  /**
   * Load permissions from server response or fall back to role-based mapping.
   * Called after successful login or token refresh.
   */
  loadPermissions(roles: Role[], serverPermissions?: Permission[]): void {
    this.userRoles.set(roles);

    if (serverPermissions && serverPermissions.length > 0) {
      // Use server-provided permissions (dynamic, stored in DB)
      this.userPermissions.set(serverPermissions);
    } else {
      // Fall back to static role-permission mapping
      const merged = this.mergeRolePermissions(roles);
      this.userPermissions.set(merged);
    }
  }

  /**
   * Clear all permissions (on logout).
   */
  clearPermissions(): void {
    this.userPermissions.set([]);
    this.userRoles.set([]);
  }

  /**
   * Check if user has a specific permission.
   */
  hasPermission(permission: Permission): boolean {
    // SuperAdmin always has all permissions
    if (this.userRoles().includes(ROLES.SUPER_ADMIN)) {
      return true;
    }
    return this.userPermissions().includes(permission);
  }

  /**
   * Check if user has ALL of the specified permissions.
   */
  hasAllPermissions(permissions: Permission[]): boolean {
    if (this.userRoles().includes(ROLES.SUPER_ADMIN)) {
      return true;
    }
    return permissions.every(p => this.userPermissions().includes(p));
  }

  /**
   * Check if user has ANY of the specified permissions.
   */
  hasAnyPermission(permissions: Permission[]): boolean {
    if (this.userRoles().includes(ROLES.SUPER_ADMIN)) {
      return true;
    }
    return permissions.some(p => this.userPermissions().includes(p));
  }

  /**
   * Check if user has a specific role.
   */
  hasRole(role: Role): boolean {
    return this.userRoles().includes(role);
  }

  /**
   * Check if user has ANY of the specified roles.
   */
  hasAnyRole(roles: Role[]): boolean {
    return roles.some(r => this.userRoles().includes(r));
  }

  /**
   * Check if user has ALL of the specified roles.
   */
  hasAllRoles(roles: Role[]): boolean {
    return roles.every(r => this.userRoles().includes(r));
  }

  /**
   * Get the primary role (first role in the array, or default).
   */
  getPrimaryRole(): Role | null {
    const roles = this.userRoles();
    return roles.length > 0 ? roles[0] : null;
  }

  /**
   * Merge permissions from multiple roles (for multi-role users).
   * Deduplicates to produce a unique set of permissions.
   */
  private mergeRolePermissions(roles: Role[]): Permission[] {
    const permissionSet = new Set<Permission>();

    for (const role of roles) {
      const rolePerms = ROLE_PERMISSIONS[role];
      if (rolePerms) {
        rolePerms.forEach(p => permissionSet.add(p));
      }
    }

    return Array.from(permissionSet);
  }
}
