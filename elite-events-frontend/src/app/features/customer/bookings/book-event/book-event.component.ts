import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BookingService } from '@core/services/booking.service';
import { EventService } from '@core/services/event.service';
import { Event } from '@core/models/event.model';
import { BookingCreateRequest } from '@core/models/booking.model';
import { ToastrService } from 'ngx-toastr';
import { resolveImageUrl } from '@core/utils/image-url.util';

@Component({
  selector: 'app-book-event',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './book-event.component.html',
  styleUrls: ['./book-event.component.scss']
})
export class BookEventComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private bookingService = inject(BookingService);
  private eventService = inject(EventService);
  private toastr = inject(ToastrService);
  readonly resolveImageUrl = resolveImageUrl;

  event = signal<Event | null>(null);
  isSubmitting = false;
  minDate = new Date().toISOString().split('T')[0];

  bookingForm = this.fb.group({
    eventDate: ['', Validators.required],
    guestCount: [1, [Validators.required, Validators.min(1), Validators.max(1000)]],
    startTime: [''],
    endTime: [''],
    specialRequests: [''],
    couponCode: [''],
    notes: ['']
  });

  ngOnInit(): void {
    const eventId = Number(this.route.snapshot.params['eventId']);
    if (eventId) {
      this.eventService.getById(eventId).subscribe(res => {
        if (res.success && res.data) {
          this.event.set(res.data);
          // Pre-fill date from event if available
          if (res.data.startDate) {
            const date = new Date(res.data.startDate).toISOString().split('T')[0];
            this.bookingForm.patchValue({ eventDate: date });
          }
        }
      });
    }
  }

  isInvalid(field: string): boolean {
    const control = this.bookingForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  incrementGuests(): void {
    const current = this.bookingForm.get('guestCount')!.value || 1;
    this.bookingForm.patchValue({ guestCount: current + 1 });
  }

  decrementGuests(): void {
    const current = this.bookingForm.get('guestCount')!.value || 1;
    if (current > 1) {
      this.bookingForm.patchValue({ guestCount: current - 1 });
    }
  }

  getBasePrice(): number {
    const eventData = this.event();
    if (!eventData) return 0;
    const price = eventData.discountPrice || eventData.price || 0;
    const guests = this.bookingForm.get('guestCount')?.value || 1;
    return price * guests;
  }

  getTax(): number {
    return this.getBasePrice() * 0.18;
  }

  getTotal(): number {
    return this.getBasePrice() + this.getTax();
  }

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const formValue = this.bookingForm.value;
    const eventData = this.event();

    const request: BookingCreateRequest = {
      eventId: eventData?.id,
      venueId: eventData?.venueId || undefined,
      eventDate: formValue.eventDate!,
      startTime: formValue.startTime || undefined,
      endTime: formValue.endTime || undefined,
      guestCount: formValue.guestCount!,
      specialRequests: formValue.specialRequests || undefined,
      couponCode: formValue.couponCode || undefined,
      notes: formValue.notes || undefined,
      details: [
        {
          serviceName: eventData?.title || 'Event Booking',
          description: `Booking for ${eventData?.title}`,
          quantity: formValue.guestCount!,
          unitPrice: eventData?.discountPrice || eventData?.price || 0
        }
      ]
    };

    this.bookingService.create(request).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.success) {
          this.toastr.success(`Booking #${res.data?.bookingNumber} created successfully!`, 'Booking Confirmed');
          this.router.navigate(['/customer/bookings']);
        }
      },
      error: () => {
        this.isSubmitting = false;
        this.toastr.error('Failed to create booking. Please try again.');
      }
    });
  }
}
