import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { noAuthGuard } from './core/guards/no-auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  // Public pages
  {
    path: '',
    loadComponent: () => import('./features/public/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'about',
    loadComponent: () => import('./features/public/about/about.component').then(m => m.AboutComponent)
  },
  {
    path: 'services',
    loadComponent: () => import('./features/public/services/services.component').then(m => m.ServicesComponent)
  },
  {
    path: 'events',
    loadComponent: () => import('./features/public/events/events.component').then(m => m.EventsComponent)
  },
  {
    path: 'events/:id',
    loadComponent: () => import('./features/public/events/event-detail.component').then(m => m.EventDetailComponent)
  },
  {
    path: 'venues',
    loadComponent: () => import('./features/public/venues/venues.component').then(m => m.VenuesComponent)
  },
  {
    path: 'venues/:id',
    loadComponent: () => import('./features/public/venues/venue-detail.component').then(m => m.VenueDetailComponent)
  },
  {
    path: 'packages',
    loadComponent: () => import('./features/public/packages/packages.component').then(m => m.PackagesComponent)
  },
  {
    path: 'gallery',
    loadComponent: () => import('./features/public/gallery/gallery.component').then(m => m.GalleryComponent)
  },
  {
    path: 'contact',
    loadComponent: () => import('./features/public/contact/contact.component').then(m => m.ContactComponent)
  },
  {
    path: 'faq',
    loadComponent: () => import('./features/public/faq/faq.component').then(m => m.FaqComponent)
  },
  {
    path: 'testimonials',
    loadComponent: () => import('./features/public/testimonials/testimonials.component').then(m => m.TestimonialsComponent)
  },

  // Auth pages (no auth required)
  {
    path: 'auth',
    canActivate: [noAuthGuard],
    children: [
      { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
      { path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent) },
      { path: 'forgot-password', loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) },
      { path: 'reset-password', loadComponent: () => import('./features/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) },
      { path: '', redirectTo: 'login', pathMatch: 'full' }
    ]
  },

  // Unauthorized page (403)
  {
    path: 'unauthorized',
    loadComponent: () => import('./shared/components/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },

  // Admin panel (SuperAdmin, Admin, EventManager)
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['SuperAdmin', 'Admin', 'EventManager'] },
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES)
  },

  // Event Coordinator panel
  {
    path: 'coordinator',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['EventCoordinator'] },
    loadChildren: () => import('./features/coordinator/coordinator.routes').then(m => m.COORDINATOR_ROUTES)
  },

  // Vendor panel
  {
    path: 'vendor',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Vendor'] },
    loadChildren: () => import('./features/vendor/vendor.routes').then(m => m.VENDOR_ROUTES)
  },

  // Staff panel
  {
    path: 'staff',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Staff'] },
    loadChildren: () => import('./features/staff/staff.routes').then(m => m.STAFF_ROUTES)
  },

  // Volunteer panel
  {
    path: 'volunteer',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Volunteer'] },
    loadChildren: () => import('./features/volunteer/volunteer.routes').then(m => m.VOLUNTEER_ROUTES)
  },

  // Client / Organizer panel
  {
    path: 'client',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Client'] },
    loadChildren: () => import('./features/client/client.routes').then(m => m.CLIENT_ROUTES)
  },

  // Customer / Attendee dashboard (requires authentication)
  {
    path: 'customer',
    canActivate: [authGuard],
    loadChildren: () => import('./features/customer/customer.routes').then(m => m.CUSTOMER_ROUTES)
  },

  // Fallback
  { path: '**', redirectTo: '' }
];
