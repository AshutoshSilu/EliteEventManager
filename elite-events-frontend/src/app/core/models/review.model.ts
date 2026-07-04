export interface Review {
  id: number;
  customerId: number;
  customerName: string;
  customerImage?: string;
  entityType: string;
  entityId: number;
  entityName?: string;
  rating: number;
  title?: string;
  comment?: string;
  imageUrl?: string;
  reply?: string;
  repliedAt?: Date;
  isApproved: boolean;
  createdAt: Date;
}

export interface Notification {
  id: number;
  title: string;
  message: string;
  type: string;
  channel: string;
  referenceType?: string;
  referenceId?: number;
  isRead: boolean;
  readAt?: Date;
  createdAt: Date;
}
