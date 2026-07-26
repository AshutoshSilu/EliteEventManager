import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const CLIENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./client-layout.component').then(m => m.ClientLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/client-dashboard.component').then(m => m.ClientDashboardComponent)
      },
      {
        path: 'requests',
        canActivate: [permissionGuard],
        data: { permissions: ['clients.requests.create'] },
        loadComponent: () => import('./requests/client-requests.component').then(m => m.ClientRequestsComponent)
      },
      {
        path: 'progress',
        canActivate: [permissionGuard],
        data: { permissions: ['clients.progress.view'] },
        loadComponent: () => import('./progress/client-progress.component').then(m => m.ClientProgressComponent)
      },
      {
        path: 'quotations',
        canActivate: [permissionGuard],
        data: { permissions: ['clients.quotations.approve'] },
        loadComponent: () => import('./quotations/client-quotations.component').then(m => m.ClientQuotationsComponent)
      },
      {
        path: 'invoices',
        canActivate: [permissionGuard],
        data: { permissions: ['invoices.view'] },
        loadComponent: () => import('./invoices/client-invoices.component').then(m => m.ClientInvoicesComponent)
      },
      {
        path: 'messages',
        canActivate: [permissionGuard],
        data: { permissions: ['clients.communication'] },
        loadComponent: () => import('./messages/client-messages.component').then(m => m.ClientMessagesComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/client-notifications.component').then(m => m.ClientNotificationsComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/client-profile.component').then(m => m.ClientProfileComponent)
      }
    ]
  }
];
