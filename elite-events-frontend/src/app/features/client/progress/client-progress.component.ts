import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-progress',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Event Progress</h2><p>Track progress of your events.</p></div>`
})
export class ClientProgressComponent {}
