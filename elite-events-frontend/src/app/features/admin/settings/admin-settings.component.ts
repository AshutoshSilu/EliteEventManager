import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({ selector: 'app-admin-settings', standalone: true, imports: [CommonModule],
  template: `<div class="page-content"><h2>System Settings</h2><div class="card p-4"><p class="text-muted">System settings including site configuration, email templates, tax settings, payment gateway keys, and notification preferences.</p></div></div>`,
  styles: [`h2{font-weight:700;margin-bottom:24px}.card{background:white;border-radius:12px;border:1px solid #e2e8f0}`] })
export class AdminSettingsComponent {}
