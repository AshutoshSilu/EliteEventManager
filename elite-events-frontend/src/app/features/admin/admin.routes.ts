import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./admin-layout.component').then(m => m.AdminLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent)
      },
      {
        path: 'users',
        canActivate: [permissionGuard],
        data: { permissions: ['users.view'] },
        loadComponent: () => import('./users/users-list.component').then(m => m.UsersListComponent)
      },
      {
        path: 'roles',
        canActivate: [permissionGuard],
        data: { permissions: ['roles.manage'] },
        loadComponent: () => import('./roles/roles-manage.component').then(m => m.RolesManageComponent)
      },
      {
        path: 'events',
        canActivate: [permissionGuard],
        data: { permissions: ['events.view'] },
        loadComponent: () => import('./events/events-manage.component').then(m => m.EventsManageComponent)
      },
      {
        path: 'calendar',
        canActivate: [permissionGuard],
        data: { permissions: ['events.view', 'schedules.view'], permissionMode: 'any' },
        loadComponent: () => import('./calendar/event-calendar.component').then(m => m.EventCalendarComponent)
      },
      {
        path: 'registrations',
        canActivate: [permissionGuard],
        data: { permissions: ['registrations.view'] },
        loadComponent: () => import('./registrations/registrations-manage.component').then(m => m.RegistrationsManageComponent)
      },
      {
        path: 'tickets',
        canActivate: [permissionGuard],
        data: { permissions: ['tickets.manage'] },
        loadComponent: () => import('./tickets/tickets-manage.component').then(m => m.TicketsManageComponent)
      },
      {
        path: 'venues',
        canActivate: [permissionGuard],
        data: { permissions: ['venues.manage'] },
        loadComponent: () => import('./venues/venues-manage.component').then(m => m.VenuesManageComponent)
      },
      {
        path: 'vendors',
        canActivate: [permissionGuard],
        data: { permissions: ['vendors.manage', 'vendors.approve'], permissionMode: 'any' },
        loadComponent: () => import('./vendors/vendors-manage.component').then(m => m.VendorsManageComponent)
      },
      {
        path: 'staff',
        canActivate: [permissionGuard],
        data: { permissions: ['staff.manage'] },
        loadComponent: () => import('./staff/staff-manage.component').then(m => m.StaffManageComponent)
      },
      {
        path: 'volunteers',
        canActivate: [permissionGuard],
        data: { permissions: ['volunteers.manage'] },
        loadComponent: () => import('./volunteers/volunteers-manage.component').then(m => m.VolunteersManageComponent)
      },
      {
        path: 'clients',
        canActivate: [permissionGuard],
        data: { permissions: ['clients.manage'] },
        loadComponent: () => import('./clients/clients-manage.component').then(m => m.ClientsManageComponent)
      },
      {
        path: 'budgets',
        canActivate: [permissionGuard],
        data: { permissions: ['budgets.manage'] },
        loadComponent: () => import('./budgets/budgets-manage.component').then(m => m.BudgetsManageComponent)
      },
      {
        path: 'payments',
        canActivate: [permissionGuard],
        data: { permissions: ['payments.manage'] },
        loadComponent: () => import('./payments/payments-manage.component').then(m => m.PaymentsManageComponent)
      },
      {
        path: 'invoices',
        canActivate: [permissionGuard],
        data: { permissions: ['invoices.manage'] },
        loadComponent: () => import('./invoices/invoices-manage.component').then(m => m.InvoicesManageComponent)
      },
      {
        path: 'reports',
        canActivate: [permissionGuard],
        data: { permissions: ['reports.view'] },
        loadComponent: () => import('./reports/reports.component').then(m => m.ReportsComponent)
      },
      {
        path: 'analytics',
        canActivate: [permissionGuard],
        data: { permissions: ['analytics.view'] },
        loadComponent: () => import('./analytics/analytics.component').then(m => m.AnalyticsComponent)
      },
      {
        path: 'gallery',
        canActivate: [permissionGuard],
        data: { permissions: ['gallery.manage'] },
        loadComponent: () => import('./gallery/gallery-manage.component').then(m => m.GalleryManageComponent)
      },
      {
        path: 'coupons',
        canActivate: [permissionGuard],
        data: { permissions: ['coupons.manage'] },
        loadComponent: () => import('./coupons/coupons-manage.component').then(m => m.CouponsManageComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/admin-notifications.component').then(m => m.AdminNotificationsComponent)
      },
      {
        path: 'settings',
        canActivate: [permissionGuard],
        data: { permissions: ['settings.manage'] },
        loadComponent: () => import('./settings/admin-settings.component').then(m => m.AdminSettingsComponent)
      },
      {
        path: 'bookings',
        canActivate: [permissionGuard],
        data: { permissions: ['registrations.manage'], permissionMode: 'any' },
        loadComponent: () => import('./bookings/bookings-manage.component').then(m => m.BookingsManageComponent)
      },
      {
        path: 'reviews',
        canActivate: [permissionGuard],
        data: { permissions: ['reviews.manage'] },
        loadComponent: () => import('./reviews/reviews-manage.component').then(m => m.ReviewsManageComponent)
      }
    ]
  }
];
