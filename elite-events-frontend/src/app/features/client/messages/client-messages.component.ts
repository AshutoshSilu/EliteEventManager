import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-messages',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Messages</h2><p>Communicate with Event Managers.</p></div>`
})
export class ClientMessagesComponent {}
