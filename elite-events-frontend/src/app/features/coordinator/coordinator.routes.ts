import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const COORDINATOR_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./coordinator-layout.component').then(m => m.CoordinatorLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/coordinator-dashboard.component').then(m => m.CoordinatorDashboardComponent)
      },
      {
        path: 'events',
        canActivate: [permissionGuard],
        data: { permissions: ['events.view'] },
        loadComponent: () => import('./events/coordinator-events.component').then(m => m.CoordinatorEventsComponent)
      },
      {
        path: 'registrations',
        canActivate: [permissionGuard],
        data: { permissions: ['registrations.manage'] },
        loadComponent: () => import('./registrations/registrations.component').then(m => m.RegistrationsComponent)
      },
      {
        path: 'checkin',
        canActivate: [permissionGuard],
        data: { permissions: ['registrations.checkin'] },
        loadComponent: () => import('./checkin/checkin.component').then(m => m.CheckinComponent)
      },
      {
        path: 'volunteers',
        canActivate: [permissionGuard],
        data: { permissions: ['volunteers.assign'] },
        loadComponent: () => import('./volunteers/coordinator-volunteers.component').then(m => m.CoordinatorVolunteersComponent)
      },
      {
        path: 'schedules',
        canActivate: [permissionGuard],
        data: { permissions: ['schedules.manage'] },
        loadComponent: () => import('./schedules/schedules.component').then(m => m.SchedulesComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/coordinator-notifications.component').then(m => m.CoordinatorNotificationsComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/coordinator-profile.component').then(m => m.CoordinatorProfileComponent)
      }
    ]
  }
];
