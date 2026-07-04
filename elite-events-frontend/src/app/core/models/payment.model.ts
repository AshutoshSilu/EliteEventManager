export interface Payment {
  id: number;
  paymentNumber: string;
  bookingId: number;
  bookingNumber: string;
  customerId: number;
  customerName: string;
  amount: number;
  paymentMethod: string;
  transactionId?: string;
  status: string;
  paymentDate?: Date;
  refundAmount?: number;
  refundDate?: Date;
  refundReason?: string;
  notes?: string;
  createdAt: Date;
}

export interface Invoice {
  id: number;
  invoiceNumber: string;
  bookingId: number;
  bookingNumber: string;
  customerName: string;
  subTotal: number;
  taxAmount: number;
  discountAmount: number;
  totalAmount: number;
  paidAmount: number;
  dueAmount: number;
  dueDate?: string;
  status: string;
  issuedAt: Date;
  paidAt?: Date;
}

export interface DashboardKpi {
  totalUsers: number;
  totalCustomers: number;
  totalBookings: number;
  todaysBookings: number;
  totalRevenue: number;
  pendingPayments: number;
  upcomingEvents: number;
  activeVendors: number;
}

export interface MonthlySales {
  monthNum: number;
  monthName: string;
  bookingCount: number;
  revenue: number;
}
