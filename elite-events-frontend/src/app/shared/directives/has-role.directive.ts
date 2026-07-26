import { Directive, Input, TemplateRef, ViewContainerRef, inject, effect, signal } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';
import { Role } from '@core/constants/permissions';

/**
 * Structural directive that conditionally renders content based on user roles.
 *
 * Usage:
 *   <button *appHasRole="'SuperAdmin'">Super Admin Only</button>
 *
 *   <!-- Multiple roles (ANY match) -->
 *   <div *appHasRole="['Admin', 'EventManager']">Admin Area</div>
 *
 *   <!-- ALL mode (user must have all listed roles) -->
 *   <div *appHasRole="['Admin', 'EventManager']; mode: 'all'">...</div>
 *
 *   <!-- With else template -->
 *   <div *appHasRole="'Admin'; else notAdmin">Admin Content</div>
 *   <ng-template #notAdmin><span>Not an admin</span></ng-template>
 */
@Directive({
  selector: '[appHasRole]',
  standalone: true,
})
export class HasRoleDirective {
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private permissionService = inject(PermissionService);

  private roles = signal<Role[]>([]);
  private mode = signal<'all' | 'any'>('any');
  private elseTemplateRef = signal<TemplateRef<any> | null>(null);

  constructor() {
    // Reactively update the view when roles change
    effect(() => {
      const roleList = this.roles();
      const checkMode = this.mode();
      const elseRef = this.elseTemplateRef();

      // Trigger reactive read of the permission service state
      this.permissionService.roles();

      const hasAccess = this.checkAccess(roleList, checkMode);
      this.updateView(hasAccess, elseRef);
    });
  }

  @Input()
  set appHasRole(value: Role | Role[]) {
    const roleList = Array.isArray(value) ? value : [value];
    this.roles.set(roleList);
  }

  @Input()
  set appHasRoleMode(value: 'all' | 'any') {
    this.mode.set(value);
  }

  @Input()
  set appHasRoleElse(templateRef: TemplateRef<any>) {
    this.elseTemplateRef.set(templateRef);
  }

  private checkAccess(roles: Role[], mode: 'all' | 'any'): boolean {
    if (roles.length === 0) return true;

    return mode === 'all'
      ? this.permissionService.hasAllRoles(roles)
      : this.permissionService.hasAnyRole(roles);
  }

  private updateView(hasAccess: boolean, elseRef: TemplateRef<any> | null): void {
    this.viewContainer.clear();

    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else if (elseRef) {
      this.viewContainer.createEmbeddedView(elseRef);
    }
  }
}
