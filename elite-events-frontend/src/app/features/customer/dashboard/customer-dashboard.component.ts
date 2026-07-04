import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { BookingService } from '@core/services/booking.service';
import { NotificationService } from '@core/services/notification.service';
import { BookingListItem } from '@core/models/booking.model';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.scss']
})
export class CustomerDashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private bookingService = inject(BookingService);
  private notificationService = inject(NotificationService);

  user = this.authService.user;
  recentBookings = signal<BookingListItem[]>([]);
  unreadCount = signal(0);
  confirmedCount = signal(0);
  pendingCount = signal(0);

  ngOnInit(): void {
    this.loadBookings();
    this.loadNotifications();
  }

  private loadBookings(): void {
    this.bookingService.getMyBookings().subscribe(res => {
      if (res.success && res.data) {
        this.recentBookings.set(res.data);
        this.confirmedCount.set(res.data.filter(b => b.status === 'Confirmed' || b.status === 'Completed').length);
        this.pendingCount.set(res.data.filter(b => b.status === 'Pending').length);
      }
    });
  }

  private loadNotifications(): void {
    this.notificationService.getUnreadCount().subscribe(res => {
      if (res.success && res.data !== undefined) this.unreadCount.set(res.data);
    });
  }
}
