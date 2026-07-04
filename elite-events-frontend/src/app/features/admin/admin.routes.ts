import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent)
  },
  {
    path: 'users',
    loadComponent: () => import('./users/users-list.component').then(m => m.UsersListComponent)
  },
  {
    path: 'events',
    loadComponent: () => import('./events/events-manage.component').then(m => m.EventsManageComponent)
  },
  {
    path: 'bookings',
    loadComponent: () => import('./bookings/bookings-manage.component').then(m => m.BookingsManageComponent)
  },
  {
    path: 'venues',
    loadComponent: () => import('./venues/venues-manage.component').then(m => m.VenuesManageComponent)
  },
  {
    path: 'vendors',
    loadComponent: () => import('./vendors/vendors-manage.component').then(m => m.VendorsManageComponent)
  },
  {
    path: 'payments',
    loadComponent: () => import('./payments/payments-manage.component').then(m => m.PaymentsManageComponent)
  },
  {
    path: 'reviews',
    loadComponent: () => import('./reviews/reviews-manage.component').then(m => m.ReviewsManageComponent)
  },
  {
    path: 'gallery',
    loadComponent: () => import('./gallery/gallery-manage.component').then(m => m.GalleryManageComponent)
  },
  {
    path: 'coupons',
    loadComponent: () => import('./coupons/coupons-manage.component').then(m => m.CouponsManageComponent)
  },
  {
    path: 'reports',
    loadComponent: () => import('./reports/reports.component').then(m => m.ReportsComponent)
  },
  {
    path: 'settings',
    loadComponent: () => import('./settings/admin-settings.component').then(m => m.AdminSettingsComponent)
  }
];
