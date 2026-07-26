import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const STAFF_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./staff-layout.component').then(m => m.StaffLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/staff-dashboard.component').then(m => m.StaffDashboardComponent)
      },
      {
        path: 'tasks',
        canActivate: [permissionGuard],
        data: { permissions: ['staff.tasks.view'] },
        loadComponent: () => import('./tasks/staff-tasks.component').then(m => m.StaffTasksComponent)
      },
      {
        path: 'checkin',
        canActivate: [permissionGuard],
        data: { permissions: ['registrations.checkin'] },
        loadComponent: () => import('./checkin/staff-checkin.component').then(m => m.StaffCheckinComponent)
      },
      {
        path: 'schedule',
        canActivate: [permissionGuard],
        data: { permissions: ['schedules.view'] },
        loadComponent: () => import('./schedule/staff-schedule.component').then(m => m.StaffScheduleComponent)
      },
      {
        path: 'issues',
        canActivate: [permissionGuard],
        data: { permissions: ['staff.issues.report'] },
        loadComponent: () => import('./issues/staff-issues.component').then(m => m.StaffIssuesComponent)
      },
      {
        path: 'notifications',
        loadComponent: () => import('./notifications/staff-notifications.component').then(m => m.StaffNotificationsComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/staff-profile.component').then(m => m.StaffProfileComponent)
      }
    ]
  }
];
