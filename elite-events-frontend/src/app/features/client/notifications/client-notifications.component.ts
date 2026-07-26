import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-notifications',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Notifications</h2><p>View your notifications.</p></div>`
})
export class ClientNotificationsComponent {}
