import { Permission, Role, ROLES } from './permissions';

/**
 * Represents a single menu item in the sidebar navigation.
 */
export interface SidebarMenuItem {
  label: string;
  icon: string;
  route: string;
  /** Permission(s) required to see this item. Empty = always visible to authenticated users. */
  permissions?: Permission[];
  /** If set, item is visible only to these roles (checked in addition to permissions). */
  roles?: Role[];
  /** Whether user needs ALL permissions or ANY. Default: 'any' */
  permissionMode?: 'all' | 'any';
  /** Child items for expandable menu groups */
  children?: SidebarMenuItem[];
  /** Badge count (for notifications, etc.) */
  badge?: number;
  /** Whether to use exact route matching for active state */
  exactMatch?: boolean;
}

/**
 * Sidebar menu configuration for the Admin/Manager panel.
 * Items are shown/hidden based on user permissions.
 */
export const ADMIN_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/admin',
    permissions: ['dashboard.admin'],
    exactMatch: true,
  },
  {
    label: 'User Management',
    icon: 'people',
    route: '/admin/users',
    permissions: ['users.view'],
  },
  {
    label: 'Role Management',
    icon: 'admin_panel_settings',
    route: '/admin/roles',
    permissions: ['roles.manage'],
  },
  {
    label: 'Event Management',
    icon: 'event',
    route: '/admin/events',
    permissions: ['events.view'],
  },
  {
    label: 'Event Calendar',
    icon: 'calendar_month',
    route: '/admin/calendar',
    permissions: ['events.view', 'schedules.view'],
    permissionMode: 'any',
  },
  {
    label: 'Registrations',
    icon: 'how_to_reg',
    route: '/admin/registrations',
    permissions: ['registrations.view'],
  },
  {
    label: 'Ticket Management',
    icon: 'confirmation_number',
    route: '/admin/tickets',
    permissions: ['tickets.manage'],
  },
  {
    label: 'Venue Management',
    icon: 'location_on',
    route: '/admin/venues',
    permissions: ['venues.manage'],
  },
  {
    label: 'Vendor Management',
    icon: 'storefront',
    route: '/admin/vendors',
    permissions: ['vendors.manage', 'vendors.approve'],
    permissionMode: 'any',
  },
  {
    label: 'Staff Management',
    icon: 'badge',
    route: '/admin/staff',
    permissions: ['staff.manage'],
  },
  {
    label: 'Volunteer Management',
    icon: 'volunteer_activism',
    route: '/admin/volunteers',
    permissions: ['volunteers.manage'],
  },
  {
    label: 'Client Management',
    icon: 'handshake',
    route: '/admin/clients',
    permissions: ['clients.manage'],
  },
  {
    label: 'Budget Management',
    icon: 'account_balance_wallet',
    route: '/admin/budgets',
    permissions: ['budgets.manage'],
  },
  {
    label: 'Payments',
    icon: 'payments',
    route: '/admin/payments',
    permissions: ['payments.manage'],
  },
  {
    label: 'Invoices',
    icon: 'receipt_long',
    route: '/admin/invoices',
    permissions: ['invoices.manage'],
  },
  {
    label: 'Reports',
    icon: 'summarize',
    route: '/admin/reports',
    permissions: ['reports.view'],
  },
  {
    label: 'Analytics',
    icon: 'analytics',
    route: '/admin/analytics',
    permissions: ['analytics.view'],
  },
  {
    label: 'Gallery',
    icon: 'photo_library',
    route: '/admin/gallery',
    permissions: ['gallery.manage'],
  },
  {
    label: 'Coupons',
    icon: 'local_offer',
    route: '/admin/coupons',
    permissions: ['coupons.manage'],
  },
  {
    label: 'Notifications',
    icon: 'notifications',
    route: '/admin/notifications',
    permissions: ['notifications.manage'],
  },
  {
    label: 'Settings',
    icon: 'settings',
    route: '/admin/settings',
    permissions: ['settings.manage'],
  },
];

/**
 * Sidebar menu for Event Coordinator panel.
 */
export const COORDINATOR_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/coordinator',
    exactMatch: true,
  },
  {
    label: 'Events',
    icon: 'event',
    route: '/coordinator/events',
    permissions: ['events.view'],
  },
  {
    label: 'Registrations',
    icon: 'how_to_reg',
    route: '/coordinator/registrations',
    permissions: ['registrations.manage'],
  },
  {
    label: 'Check-In',
    icon: 'fact_check',
    route: '/coordinator/checkin',
    permissions: ['registrations.checkin'],
  },
  {
    label: 'Volunteers',
    icon: 'volunteer_activism',
    route: '/coordinator/volunteers',
    permissions: ['volunteers.assign'],
  },
  {
    label: 'Schedules',
    icon: 'schedule',
    route: '/coordinator/schedules',
    permissions: ['schedules.manage'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/coordinator/profile',
  },
];

/**
 * Sidebar menu for Vendor panel.
 */
export const VENDOR_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/vendor',
    exactMatch: true,
  },
  {
    label: 'My Events',
    icon: 'event',
    route: '/vendor/events',
    permissions: ['events.view'],
  },
  {
    label: 'Products & Services',
    icon: 'inventory_2',
    route: '/vendor/products',
    permissions: ['vendors.products.manage'],
  },
  {
    label: 'Bookings',
    icon: 'book_online',
    route: '/vendor/bookings',
    permissions: ['vendors.bookings.manage'],
  },
  {
    label: 'Invoices',
    icon: 'receipt_long',
    route: '/vendor/invoices',
    permissions: ['vendors.invoices.upload'],
  },
  {
    label: 'Payments',
    icon: 'payments',
    route: '/vendor/payments',
    permissions: ['payments.view'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/vendor/profile',
    permissions: ['vendors.profile.edit'],
  },
];

/**
 * Sidebar menu for Staff panel.
 */
export const STAFF_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/staff',
    exactMatch: true,
  },
  {
    label: 'My Tasks',
    icon: 'task_alt',
    route: '/staff/tasks',
    permissions: ['staff.tasks.view'],
  },
  {
    label: 'Check-In',
    icon: 'fact_check',
    route: '/staff/checkin',
    permissions: ['registrations.checkin'],
  },
  {
    label: 'Event Schedule',
    icon: 'schedule',
    route: '/staff/schedule',
    permissions: ['schedules.view'],
  },
  {
    label: 'Report Issue',
    icon: 'report_problem',
    route: '/staff/issues',
    permissions: ['staff.issues.report'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/staff/profile',
  },
];

/**
 * Sidebar menu for Volunteer panel.
 */
export const VOLUNTEER_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/volunteer',
    exactMatch: true,
  },
  {
    label: 'My Shifts',
    icon: 'schedule',
    route: '/volunteer/shifts',
    permissions: ['volunteers.shifts.view'],
  },
  {
    label: 'My Tasks',
    icon: 'task_alt',
    route: '/volunteer/tasks',
    permissions: ['volunteers.tasks.complete'],
  },
  {
    label: 'Availability',
    icon: 'event_available',
    route: '/volunteer/availability',
    permissions: ['volunteers.availability.update'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/volunteer/profile',
  },
];

/**
 * Sidebar menu for Client / Organizer panel.
 */
export const CLIENT_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/client',
    exactMatch: true,
  },
  {
    label: 'My Event Requests',
    icon: 'event_note',
    route: '/client/requests',
    permissions: ['clients.requests.create'],
  },
  {
    label: 'Event Progress',
    icon: 'trending_up',
    route: '/client/progress',
    permissions: ['clients.progress.view'],
  },
  {
    label: 'Quotations',
    icon: 'request_quote',
    route: '/client/quotations',
    permissions: ['clients.quotations.approve'],
  },
  {
    label: 'Invoices',
    icon: 'receipt_long',
    route: '/client/invoices',
    permissions: ['invoices.view'],
  },
  {
    label: 'Messages',
    icon: 'chat',
    route: '/client/messages',
    permissions: ['clients.communication'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/client/profile',
  },
];

/**
 * Sidebar menu for Attendee / Customer panel.
 */
export const ATTENDEE_SIDEBAR_MENU: SidebarMenuItem[] = [
  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/customer',
    exactMatch: true,
  },
  {
    label: 'Browse Events',
    icon: 'explore',
    route: '/customer/events',
    permissions: ['public.events.browse'],
  },
  {
    label: 'My Bookings',
    icon: 'book_online',
    route: '/customer/bookings',
  },
  {
    label: 'My Tickets',
    icon: 'confirmation_number',
    route: '/customer/tickets',
    permissions: ['tickets.view'],
  },
  {
    label: 'Payments',
    icon: 'payment',
    route: '/customer/payments',
    permissions: ['payments.view'],
  },
  {
    label: 'Notifications',
    icon: 'notifications',
    route: '/customer/notifications',
  },
  {
    label: 'My Reviews',
    icon: 'rate_review',
    route: '/customer/reviews',
    permissions: ['reviews.create'],
  },
  {
    label: 'Profile',
    icon: 'person',
    route: '/customer/profile',
  },
  {
    label: 'Settings',
    icon: 'settings',
    route: '/customer/settings',
  },
];
