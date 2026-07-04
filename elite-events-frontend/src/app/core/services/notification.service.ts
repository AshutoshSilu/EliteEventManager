import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { ApiResponse } from '../models/api-response.model';
import { Notification } from '../models/review.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private unreadCount = signal<number>(0);
  readonly unreadNotifications = this.unreadCount.asReadonly();

  constructor(private http: HttpClient) {}

  getNotifications(unreadOnly = false): Observable<ApiResponse<Notification[]>> {
    return this.http.get<ApiResponse<Notification[]>>(
      `${API_ENDPOINTS.notifications.base}?unreadOnly=${unreadOnly}`
    );
  }

  getUnreadCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(API_ENDPOINTS.notifications.unreadCount)
      .pipe(tap(res => {
        if (res.success && res.data !== undefined) {
          this.unreadCount.set(res.data);
        }
      }));
  }

  markAsRead(id: number): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(API_ENDPOINTS.notifications.read(id), {})
      .pipe(tap(() => this.unreadCount.update(c => Math.max(0, c - 1))));
  }

  markAllAsRead(): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(API_ENDPOINTS.notifications.readAll, {})
      .pipe(tap(() => this.unreadCount.set(0)));
  }

  deleteNotification(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${API_ENDPOINTS.notifications.base}/${id}`);
  }
}
