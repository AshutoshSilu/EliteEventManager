import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { APP_CONSTANTS } from '../constants/app-constants';
import { Permission, Role, ROLE_DASHBOARD_ROUTES, ROLES } from '../constants/permissions';
import { ApiResponse } from '../models/api-response.model';
import { AuthUser, LoginRequest, LoginResponse, RegisterRequest } from '../models/user.model';
import { PermissionService } from './permission.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private permissionService = inject(PermissionService);
  private currentUser = signal<AuthUser | null>(this.loadUserFromStorage());
  private isAuthenticated = signal<boolean>(!!this.getToken());

  readonly user = this.currentUser.asReadonly();
  readonly isLoggedIn = this.isAuthenticated.asReadonly();
  readonly userRole = computed(() => this.currentUser()?.role ?? '');
  readonly userRoles = computed(() => this.currentUser()?.roles ?? []);

  constructor(private http: HttpClient, private router: Router) {
    // Restore permissions from stored user on app init
    const storedUser = this.currentUser();
    if (storedUser) {
      this.permissionService.loadPermissions(
        storedUser.roles ?? [storedUser.role as Role],
        storedUser.permissions
      );
    }
  }

  login(request: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(API_ENDPOINTS.auth.login, request)
      .pipe(tap(response => {
        if (response.success && response.data) {
          this.setSession(response.data);
        }
      }));
  }

  register(request: RegisterRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(API_ENDPOINTS.auth.register, request);
  }

  forgotPassword(email: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(API_ENDPOINTS.auth.forgotPassword, { email });
  }

  resetPassword(token: string, newPassword: string, confirmPassword: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(API_ENDPOINTS.auth.resetPassword, { token, newPassword, confirmPassword });
  }

  changePassword(currentPassword: string, newPassword: string, confirmPassword: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(API_ENDPOINTS.auth.changePassword, { currentPassword, newPassword, confirmPassword });
  }

  refreshToken(): Observable<ApiResponse<LoginResponse>> {
    const token = this.getToken();
    const refreshToken = this.getRefreshToken();
    return this.http.post<ApiResponse<LoginResponse>>(API_ENDPOINTS.auth.refreshToken, { token, refreshToken })
      .pipe(tap(response => {
        if (response.success && response.data) {
          this.setSession(response.data);
        }
      }));
  }

  logout(): void {
    this.http.post(API_ENDPOINTS.auth.logout, {}).subscribe();
    this.clearSession();
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(APP_CONSTANTS.tokenKey);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(APP_CONSTANTS.refreshTokenKey);
  }

  /**
   * Check if user has a specific role.
   */
  hasRole(role: string): boolean {
    return this.permissionService.hasRole(role as Role);
  }

  /**
   * Check if user has any of the given roles.
   */
  hasAnyRole(roles: string[]): boolean {
    return this.permissionService.hasAnyRole(roles as Role[]);
  }

  /**
   * Check if user has a specific permission.
   */
  hasPermission(permission: Permission): boolean {
    return this.permissionService.hasPermission(permission);
  }

  /**
   * Check if user has any of the given permissions.
   */
  hasAnyPermission(permissions: Permission[]): boolean {
    return this.permissionService.hasAnyPermission(permissions);
  }

  /**
   * Get the default dashboard route for the current user's primary role.
   */
  getDefaultDashboard(): string {
    const role = this.currentUser()?.role as Role;
    if (role && ROLE_DASHBOARD_ROUTES[role]) {
      return ROLE_DASHBOARD_ROUTES[role];
    }
    return '/';
  }

  private setSession(data: LoginResponse): void {
    localStorage.setItem(APP_CONSTANTS.tokenKey, data.token);
    localStorage.setItem(APP_CONSTANTS.refreshTokenKey, data.refreshToken);

    // Build roles array: use server-provided roles, or fall back to single role
    const normalizedPrimaryRole = this.normalizeRole(data.role as string);

    const roles: Role[] = data.roles && data.roles.length > 0
      ? data.roles.map(r => this.normalizeRole(r as string))
      : [normalizedPrimaryRole];

    // Build permissions: use server-provided or let PermissionService derive from roles
    const permissions: Permission[] = data.permissions ?? [];

    const user: AuthUser = {
      userId: data.userId,
      email: data.email,
      fullName: data.fullName,
      role: normalizedPrimaryRole,
      roles: roles,
      permissions: permissions,
      profileImageUrl: data.profileImageUrl
    };
    localStorage.setItem(APP_CONSTANTS.userKey, JSON.stringify(user));

    this.currentUser.set(user);
    this.isAuthenticated.set(true);

    // Load permissions into PermissionService
    this.permissionService.loadPermissions(roles, permissions);
  }

  private clearSession(): void {
    localStorage.removeItem(APP_CONSTANTS.tokenKey);
    localStorage.removeItem(APP_CONSTANTS.refreshTokenKey);
    localStorage.removeItem(APP_CONSTANTS.userKey);
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.permissionService.clearPermissions();
  }

  private loadUserFromStorage(): AuthUser | null {
    const userData = localStorage.getItem(APP_CONSTANTS.userKey);
    if (!userData) return null;

    const user = JSON.parse(userData) as AuthUser;
    if (user.role) {
      user.role = this.normalizeRole(user.role as string);
    }
    if (user.roles && user.roles.length > 0) {
      user.roles = user.roles.map(r => this.normalizeRole(r as string));
    }
    // Ensure backward compatibility: if roles/permissions arrays are missing, derive them
    if (!user.roles) {
      user.roles = user.role ? [user.role as Role] : [];
    }
    if (!user.permissions) {
      user.permissions = [];
    }
    return user;
  }

  private normalizeRole(role: string): Role {
    if (role === 'Administrator') {
      return ROLES.ADMIN;
    }
    return role as Role;
  }
}
