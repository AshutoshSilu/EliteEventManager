import { Permission, Role } from '../constants/permissions';

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  profileImageUrl?: string;
  roleId: number;
  roleName: string;
  roles: Role[];
  permissions: Permission[];
  isActive: boolean;
  isEmailVerified: boolean;
  lastLoginAt?: Date;
  createdAt: Date;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  email: string;
  fullName: string;
  role: Role;
  roles: Role[];
  permissions: Permission[];
  token: string;
  refreshToken: string;
  tokenExpiry: Date;
  profileImageUrl?: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
}

export interface AuthUser {
  userId: string;
  email: string;
  fullName: string;
  role: Role;
  roles: Role[];
  permissions: Permission[];
  profileImageUrl?: string;
}
