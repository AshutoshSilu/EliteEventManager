import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-profile',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Vendor Profile</h2><p>Update your vendor profile.</p></div>`
})
export class VendorProfileComponent {}
