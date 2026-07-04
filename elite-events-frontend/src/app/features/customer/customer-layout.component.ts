import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-customer-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './customer-layout.component.html',
  styleUrls: ['./customer-layout.component.scss']
})
export class CustomerLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private notificationService = inject(NotificationService);

  user = this.authService.user;
  unreadCount = signal(0);
  sidebarOpen = false;
  pageTitle = 'Dashboard';

  ngOnInit(): void {
    this.notificationService.getUnreadCount().subscribe(res => {
      if (res.success && res.data !== undefined) this.unreadCount.set(res.data);
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
