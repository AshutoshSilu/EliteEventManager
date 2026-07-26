import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-quotations',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Quotations</h2><p>Review and approve quotations.</p></div>`
})
export class ClientQuotationsComponent {}
