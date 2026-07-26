import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const VOLUNTEER_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./volunteer-layout.component').then(m => m.VolunteerLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/volunteer-dashboard.component').then(m => m.VolunteerDashboardComponent)
      },
      {
        path: 'shifts',
        canActivate: [permissionGuard],
        data: { permissions: ['volunteers.shifts.view'] },
        loadComponent: () => import('./shifts/volunteer-shifts.component').then(m => m.VolunteerShiftsComponent)
      },
      {
        path: 'tasks',
        canActivate: [permissionGuard],
        data: { permissions: ['volunteers.tasks.complete'] },
        loadComponent: () => import('./tasks/volunteer-tasks.component').then(m => m.VolunteerTasksComponent)
      },
      {
        path: 'availability',
        canActivate: [permissionGuard],
        data: { permissions: ['volunteers.availability.update'] },
        loadComponent: () => import('./availability/volunteer-availability.component').then(m => m.VolunteerAvailabilityComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/volunteer-notifications.component').then(m => m.VolunteerNotificationsComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/volunteer-profile.component').then(m => m.VolunteerProfileComponent)
      }
    ]
  }
];
