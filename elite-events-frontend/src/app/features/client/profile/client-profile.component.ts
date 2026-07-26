import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-profile',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Profile</h2><p>Manage your profile.</p></div>`
})
export class ClientProfileComponent {}
