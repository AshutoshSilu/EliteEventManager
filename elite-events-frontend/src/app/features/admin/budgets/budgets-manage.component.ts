import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-budgets-manage',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Budget Management</h2><p>Manage event budgets and allocations.</p></div>`
})
export class BudgetsManageComponent {}
