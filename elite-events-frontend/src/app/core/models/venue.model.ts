export interface Venue {
  id: number;
  name: string;
  description?: string;
  address: string;
  cityId?: number;
  cityName?: string;
  latitude?: number;
  longitude?: number;
  capacity: number;
  pricePerHour?: number;
  pricePerDay?: number;
  contactPerson?: string;
  contactPhone?: string;
  contactEmail?: string;
  facilities?: string;
  rules?: string;
  coverImageUrl?: string;
  rating: number;
  totalReviews: number;
  isActive: boolean;
  isFeatured: boolean;
  images: VenueImage[];
}

export interface VenueImage {
  id: number;
  imageUrl: string;
  caption?: string;
  sortOrder: number;
  isPrimary: boolean;
}

export interface VenueListItem {
  id: number;
  name: string;
  address: string;
  cityName?: string;
  capacity: number;
  pricePerDay?: number;
  coverImageUrl?: string;
  rating: number;
  totalReviews: number;
  isFeatured: boolean;
}
