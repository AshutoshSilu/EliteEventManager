export interface Booking {
  id: number;
  bookingNumber: string;
  customerId: number;
  customerName: string;
  customerEmail: string;
  eventId?: number;
  eventTitle?: string;
  venueId?: number;
  venueName?: string;
  packageId?: number;
  packageName?: string;
  eventDate: string;
  guestCount: number;
  specialRequests?: string;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  totalAmount: number;
  status: string;
  notes?: string;
  createdAt: Date;
  details: BookingDetail[];
}

export interface BookingDetail {
  id: number;
  vendorId?: number;
  vendorName?: string;
  serviceName: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  status: string;
}

export interface BookingCreateRequest {
  eventId?: number;
  venueId?: number;
  packageId?: number;
  eventDate: string;
  startTime?: string;
  endTime?: string;
  guestCount: number;
  specialRequests?: string;
  couponCode?: string;
  notes?: string;
  details: BookingDetailCreate[];
}

export interface BookingDetailCreate {
  vendorId?: number;
  serviceName: string;
  description?: string;
  quantity: number;
  unitPrice: number;
}

export interface BookingListItem {
  id: number;
  bookingNumber: string;
  customerName: string;
  eventTitle?: string;
  venueName?: string;
  eventDate: string;
  guestCount: number;
  totalAmount: number;
  status: string;
  createdAt: Date;
}
