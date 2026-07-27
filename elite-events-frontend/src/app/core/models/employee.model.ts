export type EmployeeStatus = 'Pending Onboarding' | 'Onboarded' | 'Resigned' | 'Terminated';

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  mobileNumber: string;
  roleId: number;
  joiningDate: string;
  profilePhotoDataUrl: string;
}

export interface EmployeeListItem {
  userId: string;
  employeeCode?: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  mobileNumber?: string;
  roleId: number;
  roleName: string;
  department?: string;
  designation?: string;
  address?: string;
  joiningDate: string;
  currentStatus: EmployeeStatus;
  profilePhotoUrl?: string;
  isActive: boolean;
}

export interface EmployeeDetail extends EmployeeListItem {}

export interface UpdateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  mobileNumber: string;
  roleId: number;
  joiningDate: string;
  department?: string;
  designation?: string;
  address?: string;
}
