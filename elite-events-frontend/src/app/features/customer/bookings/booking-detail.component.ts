import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookingService } from '@core/services/booking.service';
import { InvoiceService } from '@core/services/invoice.service';
import { Booking } from '@core/models/booking.model';

@Component({
  selector: 'app-booking-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './booking-detail.component.html',
  styleUrls: ['./booking-detail.component.scss']
})
export class BookingDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private bookingService = inject(BookingService);
  private invoiceService = inject(InvoiceService);
  booking = signal<Booking | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.params['id']);
    this.bookingService.getById(id).subscribe(res => {
      if (res.success && res.data) this.booking.set(res.data);
    });
  }

  downloadInvoice(): void {
    const booking = this.booking();
    if (booking) {
      this.invoiceService.generateInvoice(booking);
    }
  }
}
