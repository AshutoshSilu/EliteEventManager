export interface Event {
  id: number;
  title: string;
  description?: string;
  categoryId: number;
  categoryName: string;
  venueId?: number;
  venueName?: string;
  startDate: Date;
  endDate: Date;
  maxAttendees?: number;
  currentAttendees: number;
  availableSeats?: number;
  price: number;
  discountPrice?: number;
  status: string;
  coverImageUrl?: string;
  tags?: string;
  isFeatured: boolean;
  isPublished: boolean;
  createdAt: Date;
  images: EventImage[];
}

export interface EventImage {
  id: number;
  imageUrl: string;
  caption?: string;
  sortOrder: number;
}

export interface EventCategory {
  id: number;
  name: string;
  description?: string;
  iconUrl?: string;
  imageUrl?: string;
  isActive: boolean;
  eventCount: number;
}

export interface EventListItem {
  id: number;
  title: string;
  categoryName: string;
  venueName?: string;
  startDate: Date;
  endDate: Date;
  price: number;
  discountPrice?: number;
  availableSeats?: number;
  status: string;
  coverImageUrl?: string;
  isFeatured: boolean;
}
