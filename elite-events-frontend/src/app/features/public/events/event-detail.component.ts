import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { EventService } from '@core/services/event.service';
import { AuthService } from '@core/services/auth.service';
import { BookingService } from '@core/services/booking.service';
import { Event } from '@core/models/event.model';
import { ToastrService } from 'ngx-toastr';
import { resolveImageUrl } from '@core/utils/image-url.util';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule, HeaderComponent, FooterComponent],
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss']
})
export class EventDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private eventService = inject(EventService);
  private authService = inject(AuthService);
  private bookingService = inject(BookingService);
  private toastr = inject(ToastrService);
  readonly resolveImageUrl = resolveImageUrl;

  event = signal<Event | null>(null);
  isLoggedIn = this.authService.isLoggedIn;
  showBookingModal = false;
  isSubmitting = false;

  eventTypes = ['Wedding', 'Festival', 'Corporate', 'Live Events'];

  bookingForm = this.fb.group({
    clientName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    mobile: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
    address: ['', Validators.required],
    eventType: ['', Validators.required],
    eventDate: ['', Validators.required]
  });

  ngOnInit(): void {
    const id = Number(this.route.snapshot.params['id']);
    this.eventService.getById(id).subscribe(res => {
      if (res.success && res.data) {
        this.event.set(res.data);
        // Pre-fill date
        if (res.data.startDate) {
          const date = new Date(res.data.startDate).toISOString().split('T')[0];
          this.bookingForm.patchValue({ eventDate: date });
        }
      }
    });

    // Pre-fill user info if logged in
    const user = this.authService.user();
    if (user) {
      this.bookingForm.patchValue({
        clientName: user.fullName || '',
        email: user.email || ''
      });
    }
  }

  openBookingModal(): void {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: `/events/${this.event()?.id}` }
      });
      return;
    }
    this.showBookingModal = true;
  }

  closeBookingModal(): void {
    this.showBookingModal = false;
  }

  isInvalid(field: string): boolean {
    const control = this.bookingForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmitBooking(): void {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const form = this.bookingForm.value;
    const eventData = this.event();

    const request = {
      eventId: eventData?.id,
      venueId: eventData?.venueId || undefined,
      eventDate: form.eventDate!,
      guestCount: 1,
      specialRequests: `Client: ${form.clientName}, Email: ${form.email}, Mobile: ${form.mobile}, Address: ${form.address}, Event Type: ${form.eventType}`,
      notes: `Email: ${form.email}, Mobile: ${form.mobile}`,
      details: [
        {
          serviceName: form.eventType || 'Event Booking',
          description: `${form.eventType} booking for ${form.clientName}`,
          quantity: 1,
          unitPrice: eventData?.discountPrice || eventData?.price || 0
        }
      ]
    };

    this.bookingService.create(request).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.success) {
          this.showBookingModal = false;
          this.toastr.success(`Booking #${res.data?.bookingNumber} submitted successfully!`, 'Booking Submitted');
          this.router.navigate(['/customer/bookings']);
        }
      },
      error: () => {
        this.isSubmitting = false;
        this.toastr.error('Failed to submit booking. Please try again.');
      }
    });
  }

  getStatusColor(status: string): string {
    const map: Record<string, string> = { Published: 'success', Draft: 'secondary', Ongoing: 'warning', Completed: 'info', Cancelled: 'danger' };
    return map[status] || 'secondary';
  }
}
