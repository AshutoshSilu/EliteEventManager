import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-requests',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>My Event Requests</h2><p>Create and manage event requests.</p></div>`
})
export class ClientRequestsComponent {}
