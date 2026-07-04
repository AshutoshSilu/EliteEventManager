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

  // Customer dashboard (requires authentication)
  {
    path: 'customer',
    canActivate: [authGuard],
    loadChildren: () => import('./features/customer/customer.routes').then(m => m.CUSTOMER_ROUTES)
  },

  // Admin dashboard (requires admin/manager role)
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Administrator', 'EventManager'] },
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES)
  },

  // Fallback
  { path: '**', redirectTo: '' }
];
