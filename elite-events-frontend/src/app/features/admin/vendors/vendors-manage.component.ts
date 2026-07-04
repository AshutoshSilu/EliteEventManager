import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({ selector: 'app-vendors-manage', standalone: true, imports: [CommonModule, RouterLink],
  template: `<div class="page-content"><h2>Manage Vendors</h2><div class="card p-4"><p class="text-muted">Vendor management with CRUD operations, verification controls, and category assignment.</p></div></div>`,
  styles: [`h2{font-weight:700;margin-bottom:24px}.card{background:white;border-radius:12px;border:1px solid #e2e8f0}`] })
export class VendorsManageComponent {}
