import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '@core/services/notification.service';
import { Notification } from '@core/models/review.model';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  private notifService = inject(NotificationService);
  notifications = signal<Notification[]>([]);

  ngOnInit(): void {
    this.notifService.getNotifications().subscribe(res => {
      if (res.success && res.data) this.notifications.set(res.data);
    });
  }

  markRead(n: Notification): void {
    if (!n.isRead) {
      this.notifService.markAsRead(n.id).subscribe(() => { n.isRead = true; });
    }
  }

  markAllRead(): void {
    this.notifService.markAllAsRead().subscribe(() => {
      this.notifications.update(list => list.map(n => ({ ...n, isRead: true })));
    });
  }

  getIcon(type: string): string {
    const map: Record<string, string> = { Info: 'info', Success: 'check_circle', Warning: 'warning', Error: 'error', Reminder: 'alarm' };
    return map[type] || 'notifications';
  }
}
