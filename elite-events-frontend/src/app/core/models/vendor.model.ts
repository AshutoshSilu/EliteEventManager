export interface Vendor {
  id: number;
  userId: string;
  businessName: string;
  categoryId: number;
  categoryName: string;
  description?: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  website?: string;
  address?: string;
  cityName?: string;
  rating: number;
  totalReviews: number;
  pricePerHour?: number;
  pricePerEvent?: number;
  logoUrl?: string;
  isVerified: boolean;
  isActive: boolean;
}

export interface VendorCategory {
  id: number;
  name: string;
  description?: string;
  iconUrl?: string;
  isActive: boolean;
  vendorCount: number;
}
