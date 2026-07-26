import { Routes } from '@angular/router';

export const CUSTOMER_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./customer-layout.component').then(m => m.CustomerLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/customer-dashboard.component').then(m => m.CustomerDashboardComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/profile.component').then(m => m.ProfileComponent)
      },
      {
        path: 'bookings',
        loadComponent: () => import('./bookings/my-bookings.component').then(m => m.MyBookingsComponent)
      },
      {
        path: 'bookings/:id',
        loadComponent: () => import('./bookings/booking-detail.component').then(m => m.BookingDetailComponent)
      },
      {
        path: 'book-event/:eventId',
        loadComponent: () => import('./bookings/book-event/book-event.component').then(m => m.BookEventComponent)
      },
      {
        path: 'payments',
        loadComponent: () => import('./payments/payment-history.component').then(m => m.PaymentHistoryComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/notifications.component').then(m => m.NotificationsComponent)
      },
      {
        path: 'reviews',
        loadComponent: () => import('./reviews/my-reviews.component').then(m => m.MyReviewsComponent)
      },
      {
        path: 'settings',
        loadComponent: () => import('./settings/settings.component').then(m => m.SettingsComponent)
      }
    ]
  }
];
