export const APP_CONSTANTS = {
  tokenKey: 'elite_events_token',
  refreshTokenKey: 'elite_events_refresh_token',
  userKey: 'elite_events_user',
  themeKey: 'elite_events_theme',

  roles: {
    admin: 'Administrator',
    manager: 'EventManager',
    customer: 'Customer',
    vendor: 'Vendor',
    visitor: 'Visitor'
  },

  bookingStatuses: ['Pending', 'Confirmed', 'InProgress', 'Completed', 'Cancelled', 'Refunded'],
  paymentMethods: ['UPI', 'CreditCard', 'DebitCard', 'NetBanking', 'Cash', 'BankTransfer'],
  paymentStatuses: ['Pending', 'Processing', 'Completed', 'Failed', 'Refunded'],

  pagination: {
    defaultPageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100]
  }
};
