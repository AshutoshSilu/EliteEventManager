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

  cancelBooking(booking: BookingListItem): void {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.bookingService.cancel(booking.id, 'Customer requested cancellation').subscribe(res => {
        if (res.success) {
          this.toastr.success('Booking cancelled successfully');
          booking.status = 'Cancelled';
          this.applyFilter();
        }
      });
    }
  }
}
