import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from '@core/services/booking.service';
import { BookingListItem } from '@core/models/booking.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-bookings-manage',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page-content">
      <div class="d-flex justify-content-between align-items-center mb-4"><h2>Manage Bookings</h2></div>
      <div class="card">
        <div class="card-filters">
          <input type="text" class="form-control" placeholder="Search by booking #..." [(ngModel)]="searchTerm" (input)="loadBookings()" style="max-width:280px">
          <select class="form-select" [(ngModel)]="statusFilter" (change)="loadBookings()" style="max-width:160px">
            <option value="">All Status</option>
            <option>Pending</option><option>Confirmed</option><option>InProgress</option><option>Completed</option><option>Cancelled</option>
          </select>
        </div>
        <div class="table-responsive">
          <table class="table">
            <thead><tr><th>Booking #</th><th>Customer</th><th>Event/Venue</th><th>Date</th><th>Amount</th><th>Status</th><th>Actions</th></tr></thead>
            <tbody>
              @for (b of bookings(); track b.id) {
                <tr>
                  <td class="fw-medium text-primary">{{ b.bookingNumber }}</td>
                  <td>{{ b.customerName }}</td>
                  <td>{{ b.eventTitle || b.venueName || '—' }}</td>
                  <td>{{ b.eventDate }}</td>
                  <td class="fw-bold">{{ b.totalAmount | currency:'INR' }}</td>
                  <td><span class="status-badge" [class]="'s-' + b.status.toLowerCase()">{{ b.status }}</span></td>
                  <td>
                    @if (b.status === 'Pending') {
                      <button class="btn btn-sm btn-success-sm" (click)="approve(b)">Approve</button>
                    }
                    <button class="btn-icon" title="View"><span class="material-icons">visibility</span></button>
                  </td>
                </tr>
              } @empty {
                <tr><td colspan="7" class="text-center py-4 text-muted">No bookings found</td></tr>
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
    .text-primary { color: #6366f1 !important; }
    .status-badge { padding: 4px 10px; border-radius: 12px; font-size: 0.75rem; font-weight: 600; }
    .s-pending { background: #fef3c7; color: #92400e; } .s-confirmed { background: #d1fae5; color: #065f46; }
    .s-inprogress { background: #e0e7ff; color: #3730a3; } .s-completed { background: #dbeafe; color: #1e40af; }
    .s-cancelled { background: #fee2e2; color: #991b1b; }
    .btn-success-sm { background: #10b981; color: white; border: none; border-radius: 6px; padding: 4px 12px; font-size: 0.8rem; }
    .btn-icon { background: none; border: none; cursor: pointer; padding: 4px; color: #64748b; }
  `]
})
export class BookingsManageComponent implements OnInit {
  private bookingService = inject(BookingService);
  private toastr = inject(ToastrService);

  bookings = signal<BookingListItem[]>([]);
  searchTerm = '';
  statusFilter = '';

  ngOnInit(): void { this.loadBookings(); }

  loadBookings(): void {
    this.bookingService.getAll({
      pageNumber: 1, pageSize: 50, searchTerm: this.searchTerm,
      filterBy: this.statusFilter ? 'status' : undefined,
      filterValue: this.statusFilter || undefined
    }).subscribe(res => { if (res.success && res.data) this.bookings.set(res.data.items); });
  }

  approve(b: BookingListItem): void {
    this.bookingService.updateStatus(b.id, 'Confirmed', 'Approved by admin').subscribe(res => {
      if (res.success) { this.toastr.success('Booking approved'); b.status = 'Confirmed'; }
    });
  }
}
