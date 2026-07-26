import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookingService } from '@core/services/booking.service';
import { BookingListItem } from '@core/models/booking.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.scss']
})
export class MyBookingsComponent implements OnInit {
  private bookingService = inject(BookingService);
  private toastr = inject(ToastrService);

  bookings = signal<BookingListItem[]>([]);
  filteredBookings = signal<BookingListItem[]>([]);
  statusFilter = '';

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.bookingService.getMyBookings().subscribe(res => {
      if (res.success && res.data) {
        this.bookings.set(res.data);
        this.filteredBookings.set(res.data);
      }
    });
  }

  applyFilter(): void {
    if (this.statusFilter) {
      this.filteredBookings.set(this.bookings().filter(b => b.status === this.statusFilter));
    } else {
      this.filteredBookings.set(this.bookings());
    }
  }

  approveBooking(booking: BookingListItem): void {
    if (confirm(`Approve booking ${booking.bookingNumber}? A confirmation email will be sent to the client.`)) {
      this.bookingService.updateStatus(booking.id, 'Confirmed', 'Booking approved by customer').subscribe({
        next: (res) => {
          if (res.success) {
            booking.status = 'Confirmed';
            this.toastr.success('Booking approved! Confirmation email sent to client.', 'Approved');
            this.applyFilter();
          }
        },
        error: () => {
          this.toastr.error('Failed to approve booking.');
        }
      });
    }
  }

  denyBooking(booking: BookingListItem): void {
    if (confirm(`Deny booking ${booking.bookingNumber}? The client will be notified.`)) {
      this.bookingService.updateStatus(booking.id, 'Cancelled', 'Booking denied').subscribe({
        next: (res) => {
          if (res.success) {
            booking.status = 'Cancelled';
            this.toastr.info('Booking denied. Rejection email sent to client.', 'Denied');
            this.applyFilter();
          }
        },
        error: () => {
          this.toastr.error('Failed to deny booking.');
        }
      });
    }
  }

  getClientField(booking: BookingListItem, field: string): string {
    const source = booking.specialRequests || '';
    const parts = source.split(',');
    for (const part of parts) {
      const trimmed = part.trim();
      if (trimmed.toLowerCase().startsWith(field.toLowerCase() + ':')) {
        return trimmed.substring(field.length + 1).trim();
      }
    }
    return '';
  }
}
