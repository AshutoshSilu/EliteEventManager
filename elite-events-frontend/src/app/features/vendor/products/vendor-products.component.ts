import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-products',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Products & Services</h2><p>Manage your products and services.</p></div>`
})
export class VendorProductsComponent {}
