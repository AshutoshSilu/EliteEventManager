export const APP_CONSTANTS = {
  tokenKey: 'elite_events_token',
  refreshTokenKey: 'elite_events_refresh_token',
  userKey: 'elite_events_user',
  themeKey: 'elite_events_theme',

  roles: {
    superAdmin: 'SuperAdmin',
    admin: 'Admin',
    eventManager: 'EventManager',
    eventCoordinator: 'EventCoordinator',
    vendor: 'Vendor',
    staff: 'Staff',
    volunteer: 'Volunteer',
    client: 'Client',
    attendee: 'Attendee',
    guest: 'Guest'
  },

  bookingStatuses: ['Pending', 'Confirmed', 'InProgress', 'Completed', 'Cancelled', 'Refunded'],
  paymentMethods: ['UPI', 'CreditCard', 'DebitCard', 'NetBanking', 'Cash', 'BankTransfer'],
  paymentStatuses: ['Pending', 'Processing', 'Completed', 'Failed', 'Refunded'],

  pagination: {
    defaultPageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100]
  }
};
