import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const VENDOR_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./vendor-layout.component').then(m => m.VendorLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/vendor-dashboard.component').then(m => m.VendorDashboardComponent)
      },
      {
        path: 'events',
        canActivate: [permissionGuard],
        data: { permissions: ['events.view'] },
        loadComponent: () => import('./events/vendor-events.component').then(m => m.VendorEventsComponent)
      },
      {
        path: 'products',
        canActivate: [permissionGuard],
        data: { permissions: ['vendors.products.manage'] },
        loadComponent: () => import('./products/vendor-products.component').then(m => m.VendorProductsComponent)
      },
      {
        path: 'bookings',
        canActivate: [permissionGuard],
        data: { permissions: ['vendors.bookings.manage'] },
        loadComponent: () => import('./bookings/vendor-bookings.component').then(m => m.VendorBookingsComponent)
      },
      {
        path: 'invoices',
        canActivate: [permissionGuard],
        data: { permissions: ['vendors.invoices.upload'] },
        loadComponent: () => import('./invoices/vendor-invoices.component').then(m => m.VendorInvoicesComponent)
      },
      {
        path: 'payments',
        canActivate: [permissionGuard],
        data: { permissions: ['payments.view'] },
        loadComponent: () => import('./payments/vendor-payments.component').then(m => m.VendorPaymentsComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/vendor-notifications.component').then(m => m.VendorNotificationsComponent)
      },
      {
        path: 'profile',
        canActivate: [permissionGuard],
        data: { permissions: ['vendors.profile.edit'] },
        loadComponent: () => import('./profile/vendor-profile.component').then(m => m.VendorProfileComponent)
      }
    ]
  }
];
