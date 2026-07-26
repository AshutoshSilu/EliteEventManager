import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coordinator-volunteers',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page"><h2>Volunteers</h2><p>Assign and manage volunteers.</p></div>`
})
export class CoordinatorVolunteersComponent {}
