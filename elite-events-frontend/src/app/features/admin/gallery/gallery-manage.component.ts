import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({ selector: 'app-gallery-manage', standalone: true, imports: [CommonModule],
  template: `<div class="page-content"><h2>Manage Gallery</h2><div class="card p-4"><p class="text-muted">Gallery management with drag-and-drop upload, album creation, and media organization.</p></div></div>`,
  styles: [`h2{font-weight:700;margin-bottom:24px}.card{background:white;border-radius:12px;border:1px solid #e2e8f0}`] })
export class GalleryManageComponent {}
