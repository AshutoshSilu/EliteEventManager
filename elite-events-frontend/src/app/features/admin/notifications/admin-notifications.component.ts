import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-notifications',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Notifications</h2><p>Manage system notifications.</p></div>`
})
export class AdminNotificationsComponent {}
