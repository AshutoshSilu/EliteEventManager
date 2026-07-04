import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EventService } from '@core/services/event.service';
import { EventListItem } from '@core/models/event.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-events-manage',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="page-content">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Manage Events</h2>
        <button class="btn btn-primary-custom btn-sm">+ Create Event</button>
      </div>
      <div class="card">
        <div class="card-filters">
          <input type="text" class="form-control" placeholder="Search events..." [(ngModel)]="searchTerm" (input)="loadEvents()" style="max-width:280px">
          <select class="form-select" [(ngModel)]="statusFilter" (change)="loadEvents()" style="max-width:160px">
            <option value="">All Status</option>
            <option>Draft</option><option>Published</option><option>Ongoing</option><option>Completed</option><option>Cancelled</option>
          </select>
        </div>
        <div class="table-responsive">
          <table class="table">
            <thead><tr><th>Title</th><th>Category</th><th>Date</th><th>Price</th><th>Status</th><th>Actions</th></tr></thead>
            <tbody>
              @for (event of events(); track event.id) {
                <tr>
                  <td class="fw-medium">{{ event.title }}</td>
                  <td>{{ event.categoryName }}</td>
                  <td>{{ event.startDate | date:'mediumDate' }}</td>
                  <td>{{ event.price | currency:'INR' }}</td>
                  <td><span class="status-badge" [class]="'s-' + event.status.toLowerCase()">{{ event.status }}</span></td>
                  <td>
                    <button class="btn-icon" title="Edit"><span class="material-icons">edit</span></button>
                    <button class="btn-icon text-danger" title="Delete" (click)="deleteEvent(event)"><span class="material-icons">delete</span></button>
                  </td>
                </tr>
              } @empty {
                <tr><td colspan="6" class="text-center py-4 text-muted">No events found</td></tr>
              }
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    h2 { font-weight: 700; }
    .card { background: white; border-radius: 12px; border: 1px solid #e2e8f0; overflow: hidden; }
    .card-filters { display: flex; gap: 12px; padding: 16px 20px; border-bottom: 1px solid #f1f5f9; }
    .form-control, .form-select { border-radius: 8px; border: 1.5px solid #e2e8f0; padding: 8px 14px; }
    .table { margin: 0; } .table th { font-size: 0.8rem; text-transform: uppercase; color: #64748b; padding: 12px 16px; }
    .table td { padding: 12px 16px; vertical-align: middle; }
    .status-badge { padding: 4px 10px; border-radius: 12px; font-size: 0.75rem; font-weight: 600; }
    .s-draft { background: #f1f5f9; color: #475569; } .s-published { background: #d1fae5; color: #065f46; }
    .s-ongoing { background: #fef3c7; color: #92400e; } .s-completed { background: #dbeafe; color: #1e40af; }
    .s-cancelled { background: #fee2e2; color: #991b1b; }
    .btn-icon { background: none; border: none; cursor: pointer; padding: 4px; color: #64748b; }
    .btn-icon:hover { color: #6366f1; } .btn-icon.text-danger:hover { color: #ef4444; }
  `]
})
export class EventsManageComponent implements OnInit {
  private eventService = inject(EventService);
  private toastr = inject(ToastrService);

  events = signal<EventListItem[]>([]);
  searchTerm = '';
  statusFilter = '';

  ngOnInit(): void { this.loadEvents(); }

  loadEvents(): void {
    this.eventService.getAll({ pageNumber: 1, pageSize: 50, searchTerm: this.searchTerm }).subscribe(res => {
      if (res.success && res.data) this.events.set(res.data.items);
    });
  }

  deleteEvent(event: EventListItem): void {
    if (confirm(`Delete event "${event.title}"?`)) {
      this.eventService.delete(event.id).subscribe(res => {
        if (res.success) { this.toastr.success('Event deleted'); this.loadEvents(); }
      });
    }
  }
}
