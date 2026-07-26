import { Directive, Input, TemplateRef, ViewContainerRef, inject, effect, signal } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';
import { Permission } from '@core/constants/permissions';

/**
 * Structural directive that conditionally renders content based on user permissions.
 *
 * Usage:
 *   <button *appHasPermission="'events.create'">Create Event</button>
 *
 *   <!-- Multiple permissions (ALL required by default) -->
 *   <div *appHasPermission="['events.create', 'events.publish']">...</div>
 *
 *   <!-- ANY mode -->
 *   <div *appHasPermission="['events.create', 'events.edit']; mode: 'any'">...</div>
 *
 *   <!-- With else template -->
 *   <div *appHasPermission="'events.delete'; else noAccess">Delete</div>
 *   <ng-template #noAccess><span>No access</span></ng-template>
 */
@Directive({
  selector: '[appHasPermission]',
  standalone: true,
})
export class HasPermissionDirective {
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private permissionService = inject(PermissionService);

  private permissions = signal<Permission[]>([]);
  private mode = signal<'all' | 'any'>('all');
  private elseTemplateRef = signal<TemplateRef<any> | null>(null);
  private isRendered = false;

  constructor() {
    // Reactively update the view when permissions change
    effect(() => {
      const perms = this.permissions();
      const checkMode = this.mode();
      const elseRef = this.elseTemplateRef();

      // Trigger reactive read of the permission service state
      this.permissionService.permissions();

      const hasAccess = this.checkAccess(perms, checkMode);
      this.updateView(hasAccess, elseRef);
    });
  }

  @Input()
  set appHasPermission(value: Permission | Permission[]) {
    const perms = Array.isArray(value) ? value : [value];
    this.permissions.set(perms);
  }

  @Input()
  set appHasPermissionMode(value: 'all' | 'any') {
    this.mode.set(value);
  }

  @Input()
  set appHasPermissionElse(templateRef: TemplateRef<any>) {
    this.elseTemplateRef.set(templateRef);
  }

  private checkAccess(permissions: Permission[], mode: 'all' | 'any'): boolean {
    if (permissions.length === 0) return true;

    return mode === 'any'
      ? this.permissionService.hasAnyPermission(permissions)
      : this.permissionService.hasAllPermissions(permissions);
  }

  private updateView(hasAccess: boolean, elseRef: TemplateRef<any> | null): void {
    this.viewContainer.clear();
    this.isRendered = false;

    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.isRendered = true;
    } else if (elseRef) {
      this.viewContainer.createEmbeddedView(elseRef);
    }
  }
}
