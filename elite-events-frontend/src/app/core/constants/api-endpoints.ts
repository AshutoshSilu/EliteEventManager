import { environment } from '@environments/environment';

const BASE = environment.apiUrl;

export const API_ENDPOINTS = {
  auth: {
    login: `${BASE}/auth/login`,
    register: `${BASE}/auth/register`,
    refreshToken: `${BASE}/auth/refresh-token`,
    forgotPassword: `${BASE}/auth/forgot-password`,
    resetPassword: `${BASE}/auth/reset-password`,
    changePassword: `${BASE}/auth/change-password`,
    verifyEmail: `${BASE}/auth/verify-email`,
    logout: `${BASE}/auth/logout`
  },
  users: {
    base: `${BASE}/users`,
    profile: `${BASE}/users/profile`,
    byRole: (roleId: number) => `${BASE}/users/role/${roleId}`
  },
  employees: {
    base: `${BASE}/employees`,
    byUserId: (userId: string) => `${BASE}/employees/${userId}`,
    onboard: (userId: string) => `${BASE}/employees/${userId}/onboard`,
    resign: (userId: string) => `${BASE}/employees/${userId}/resign`,
    terminate: (userId: string) => `${BASE}/employees/${userId}/terminate`
  },
  events: {
    base: `${BASE}/events`,
    featured: `${BASE}/events/featured`,
    upcoming: `${BASE}/events/upcoming`,
    categories: `${BASE}/events/categories`,
    byCategory: (id: number) => `${BASE}/events/category/${id}`,
    search: `${BASE}/events/search`,
    byId: (id: number) => `${BASE}/events/${id}`
  },
  bookings: {
    base: `${BASE}/bookings`,
    myBookings: `${BASE}/bookings/my-bookings`,
    todayCount: `${BASE}/bookings/today-count`,
    byNumber: (num: string) => `${BASE}/bookings/number/${num}`,
    status: (id: number) => `${BASE}/bookings/${id}/status`,
    cancel: (id: number) => `${BASE}/bookings/${id}/cancel`
  },
  venues: {
    base: `${BASE}/venues`,
    featured: `${BASE}/venues/featured`,
    search: `${BASE}/venues/search`,
    byId: (id: number) => `${BASE}/venues/${id}`,
    availability: (id: number) => `${BASE}/venues/${id}/availability`,
    calendar: (id: number) => `${BASE}/venues/${id}/calendar`
  },
  vendors: {
    base: `${BASE}/vendors`,
    categories: `${BASE}/vendors/categories`,
    topRated: `${BASE}/vendors/top-rated`,
    byCategory: (id: number) => `${BASE}/vendors/category/${id}`,
    byId: (id: number) => `${BASE}/vendors/${id}`
  },
  payments: {
    base: `${BASE}/payments`,
    myPayments: `${BASE}/payments/my-payments`,
    refund: `${BASE}/payments/refund`,
    invoice: (bookingId: number) => `${BASE}/payments/invoice/${bookingId}`,
    byBooking: (bookingId: number) => `${BASE}/payments/booking/${bookingId}`
  },
  reviews: {
    base: `${BASE}/reviews`,
    pending: `${BASE}/reviews/pending`,
    byEntity: (type: string, id: number) => `${BASE}/reviews/entity/${type}/${id}`,
    reply: `${BASE}/reviews/reply`,
    approve: (id: number) => `${BASE}/reviews/${id}/approve`
  },
  notifications: {
    base: `${BASE}/notifications`,
    unreadCount: `${BASE}/notifications/unread-count`,
    readAll: `${BASE}/notifications/read-all`,
    read: (id: number) => `${BASE}/notifications/${id}/read`
  },
  reports: {
    dashboardKpis: `${BASE}/reports/dashboard-kpis`,
    revenue: `${BASE}/reports/revenue`,
    bookings: `${BASE}/reports/bookings`,
    monthlySales: (year: number) => `${BASE}/reports/monthly-sales/${year}`
  }
};
