import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { APP_CONSTANTS } from '../constants/app-constants';
import { ApiResponse } from '../models/api-response.model';
import { AuthUser, LoginRequest, LoginResponse, RegisterRequest } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUser = signal<AuthUser | null>(this.loadUserFromStorage());
  private isAuthenticated = signal<boolean>(!!this.getToken());

  readonly user = this.currentUser.asReadonly();
  readonly isLoggedIn = this.isAuthenticated.asReadonly();
  readonly userRole = computed(() => this.currentUser()?.role ?? '');

  constructor(private http: HttpClient, private router: Router) {}

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

  hasRole(role: string): boolean {
    return this.currentUser()?.role === role;
  }

  hasAnyRole(roles: string[]): boolean {
    const userRole = this.currentUser()?.role;
    return userRole ? roles.includes(userRole) : false;
  }

  private setSession(data: LoginResponse): void {
    localStorage.setItem(APP_CONSTANTS.tokenKey, data.token);
    localStorage.setItem(APP_CONSTANTS.refreshTokenKey, data.refreshToken);

    const user: AuthUser = {
      userId: data.userId,
      email: data.email,
      fullName: data.fullName,
      role: data.role,
      profileImageUrl: data.profileImageUrl
    };
    localStorage.setItem(APP_CONSTANTS.userKey, JSON.stringify(user));

    this.currentUser.set(user);
    this.isAuthenticated.set(true);
  }

  private clearSession(): void {
    localStorage.removeItem(APP_CONSTANTS.tokenKey);
    localStorage.removeItem(APP_CONSTANTS.refreshTokenKey);
    localStorage.removeItem(APP_CONSTANTS.userKey);
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
  }

  private loadUserFromStorage(): AuthUser | null {
    const userData = localStorage.getItem(APP_CONSTANTS.userKey);
    return userData ? JSON.parse(userData) : null;
  }
}
