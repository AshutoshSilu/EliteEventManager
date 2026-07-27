import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EmployeeDetail, EmployeeListItem } from '@core/models/employee.model';
import { EmployeeService } from '@core/services/employee.service';
import { resolveImageUrl } from '@core/utils/image-url.util';
import { jsPDF } from 'jspdf';

interface RoleOption {
  id: number;
  label: string;
}

@Component({
  selector: 'app-employee-registration',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './employee-registration.component.html',
  styleUrls: ['./employee-registration.component.scss']
})
export class EmployeeRegistrationComponent implements OnInit {
  private fb = inject(FormBuilder);
  private employeeService = inject(EmployeeService);
  private toastr = inject(ToastrService);
  readonly resolveImageUrl = resolveImageUrl;

  employees = signal<EmployeeListItem[]>([]);
  searchTerm = '';
  roleFilter = '';
  statusFilter = '';
  isLoadingEmployees = false;
  showRegisterPopup = false;
  showDetailsPopup = false;
  isLoadingDetails = false;
  isEditingDetails = false;
  isSavingDetails = false;
  isGeneratingIdCard = false;
  processingUserId: string | null = null;
  selectedEmployee = signal<EmployeeDetail | null>(null);

  filteredEmployees = computed(() => {
    const search = this.searchTerm.trim().toLowerCase();
    const role = this.roleFilter;
    const status = this.statusFilter;

    return this.employees().filter((employee) => {
      const matchesSearch = !search ||
        employee.fullName.toLowerCase().includes(search) ||
        employee.email.toLowerCase().includes(search) ||
        (employee.mobileNumber || '').toLowerCase().includes(search);

      const matchesRole = !role || employee.roleName === role;
      const matchesStatus = !status || employee.currentStatus === status;

      return matchesSearch && matchesRole && matchesStatus;
    });
  });

  availableRoles = computed(() => {
    const roles = this.employees().map(e => e.roleName);
    return Array.from(new Set(roles)).sort((a, b) => a.localeCompare(b));
  });

  isSubmitting = false;
  photoPreview?: string;

  readonly roleOptions: RoleOption[] = [
    { id: 1, label: 'Administrator' },
    { id: 2, label: 'Event Manager' },
    { id: 3, label: 'Customer' },
    { id: 4, label: 'Vendor' }
  ];

  employeeForm = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email]],
    mobileNumber: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s]{8,20}$/)]],
    roleId: [null as number | null, [Validators.required]],
    joiningDate: ['', [Validators.required]],
    profilePhoto: ['', [Validators.required]]
  });

  employeeDetailsForm = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email]],
    mobileNumber: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s]{8,20}$/)]],
    roleId: [null as number | null, [Validators.required]],
    joiningDate: ['', [Validators.required]],
    department: [''],
    designation: [''],
    address: ['']
  });

  ngOnInit(): void {
    this.loadEmployees();
  }

  openRegisterPopup(): void {
    this.showRegisterPopup = true;
  }

  closeRegisterPopup(): void {
    this.showRegisterPopup = false;
    this.employeeForm.reset();
    this.photoPreview = undefined;
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.roleFilter = '';
    this.statusFilter = '';
  }

  private loadEmployees(): void {
    this.isLoadingEmployees = true;
    this.employeeService.getAll({ pageSize: 1000, sortBy: 'joiningDate', sortDirection: 'desc' }).subscribe({
      next: (res) => {
        this.isLoadingEmployees = false;
        if (res.success && res.data) {
          this.employees.set(res.data.items || []);
        }
      },
      error: () => {
        this.isLoadingEmployees = false;
      }
    });
  }

  onboardEmployee(userId: string): void {
    if (this.processingUserId) {
      return;
    }

    this.processingUserId = userId;
    this.employeeService.onboard(userId).subscribe({
      next: (response) => {
        this.processingUserId = null;
        if (!response.success) {
          this.toastr.error(response.message || 'Unable to onboard employee.');
          return;
        }

        this.toastr.success('Employee onboarded successfully. Welcome letter email sent with attachment.', 'Success');
        this.loadEmployees();
      },
      error: () => {
        this.processingUserId = null;
      }
    });
  }

  terminateEmployee(userId: string): void {
    if (this.processingUserId) {
      return;
    }

    this.processingUserId = userId;
    this.employeeService.terminate(userId).subscribe({
      next: (response) => {
        this.processingUserId = null;
        if (!response.success) {
          this.toastr.error(response.message || 'Unable to terminate employee.');
          return;
        }

        this.toastr.success('Employee terminated successfully.', 'Success');
        this.loadEmployees();
      },
      error: () => {
        this.processingUserId = null;
      }
    });
  }

  openEmployeeDetails(userId: string): void {
    this.showDetailsPopup = true;
    this.isLoadingDetails = true;
    this.isEditingDetails = false;
    this.selectedEmployee.set(null);

    this.employeeService.getByUserId(userId).subscribe({
      next: (response) => {
        this.isLoadingDetails = false;
        if (!response.success || !response.data) {
          this.toastr.error(response.message || 'Unable to fetch employee details.');
          this.closeEmployeeDetails();
          return;
        }

        this.selectedEmployee.set(response.data);
      },
      error: () => {
        this.isLoadingDetails = false;
        this.closeEmployeeDetails();
      }
    });
  }

  closeEmployeeDetails(): void {
    this.showDetailsPopup = false;
    this.isLoadingDetails = false;
    this.isEditingDetails = false;
    this.selectedEmployee.set(null);
    this.employeeDetailsForm.reset();
  }

  startEditDetails(): void {
    const employee = this.selectedEmployee();
    if (!employee) {
      return;
    }

    this.employeeDetailsForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      mobileNumber: employee.mobileNumber || '',
      roleId: employee.roleId,
      joiningDate: this.toDateInputValue(employee.joiningDate),
      department: employee.department || '',
      designation: employee.designation || '',
      address: employee.address || ''
    });
    this.isEditingDetails = true;
  }

  cancelEditDetails(): void {
    this.isEditingDetails = false;
    this.employeeDetailsForm.reset();
  }

  isDetailFieldInvalid(field: string): boolean {
    const control = this.employeeDetailsForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  saveEmployeeDetails(): void {
    const employee = this.selectedEmployee();
    if (!employee) {
      return;
    }

    if (this.employeeDetailsForm.invalid) {
      this.employeeDetailsForm.markAllAsTouched();
      return;
    }

    const formValue = this.employeeDetailsForm.getRawValue();
    this.isSavingDetails = true;

    this.employeeService.update(employee.userId, {
      firstName: formValue.firstName!,
      lastName: formValue.lastName!,
      email: formValue.email!,
      mobileNumber: formValue.mobileNumber!,
      roleId: formValue.roleId!,
      joiningDate: formValue.joiningDate!,
      department: formValue.department || undefined,
      designation: formValue.designation || undefined,
      address: formValue.address || undefined
    }).subscribe({
      next: (response) => {
        this.isSavingDetails = false;
        if (!response.success || !response.data) {
          this.toastr.error(response.message || 'Unable to update employee details.');
          return;
        }

        this.selectedEmployee.set(response.data);
        this.isEditingDetails = false;
        this.employeeDetailsForm.reset();
        this.toastr.success('Employee details updated successfully.', 'Success');
        this.loadEmployees();
      },
      error: () => {
        this.isSavingDetails = false;
      }
    });
  }

  async generateIdCardPdf(): Promise<void> {
    const employee = this.selectedEmployee();
    if (!employee || this.isGeneratingIdCard) {
      return;
    }

    this.isGeneratingIdCard = true;
    try {
      const doc = new jsPDF({ orientation: 'portrait', unit: 'mm', format: [54, 86] });

      doc.setFillColor(6, 95, 100);
      doc.rect(0, 0, 54, 86, 'F');
      doc.setFillColor(190, 24, 93);
      doc.circle(48, 10, 12, 'F');
      doc.setFillColor(255, 255, 255);
      doc.roundedRect(3.5, 3.5, 47, 79, 4, 4, 'F');

      doc.setFillColor(8, 145, 178);
      doc.roundedRect(3.5, 3.5, 47, 17, 4, 4, 'F');
      doc.setFillColor(190, 24, 93);
      doc.rect(3.5, 18.5, 47, 2.2, 'F');

      doc.setFont('helvetica', 'bold');
      doc.setFontSize(12);
      doc.setTextColor(255, 255, 255);
      doc.text('ATAV Events', 27, 10.5, { align: 'center' });
      doc.setFontSize(6.5);
      doc.setFont('helvetica', 'normal');
      doc.text('Creative Talent Identity Card', 27, 14.5, { align: 'center' });

      const profileUrl = resolveImageUrl(employee.profilePhotoUrl, '');
      const imageDataUrl = await this.loadImageAsDataUrl(profileUrl);
      if (imageDataUrl) {
        doc.setDrawColor(255, 255, 255);
        doc.setFillColor(255, 255, 255);
        doc.circle(27, 34, 12.5, 'F');
        doc.addImage(imageDataUrl, 'JPEG', 16.5, 23.5, 21, 21, undefined, 'FAST');
      } else {
        doc.setDrawColor(203, 213, 225);
        doc.setFillColor(248, 250, 252);
        doc.circle(27, 34, 12.5, 'F');
        doc.setFontSize(7.5);
        doc.setTextColor(100, 116, 139);
        doc.text('No Photo', 27, 35, { align: 'center' });
      }

      doc.setTextColor(15, 23, 42);
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(9.8);
      doc.text(employee.fullName, 27, 49, { align: 'center', maxWidth: 42 });

      doc.setFillColor(236, 253, 245);
      doc.roundedRect(7, 53, 40, 11, 2.5, 2.5, 'F');
      doc.setFontSize(7.8);
      doc.setFont('helvetica', 'normal');
      doc.setTextColor(13, 148, 136);
      doc.text(`Emp:- ${employee.employeeCode || 'N/A'}`, 27, 57, { align: 'center' });
      doc.text(`${employee.designation || employee.roleName}`, 27, 61, { align: 'center' });

      doc.setFillColor(253, 242, 248);
      doc.roundedRect(7, 66, 40, 12, 2.5, 2.5, 'F');
      doc.setTextColor(190, 24, 93);
      doc.text(`Mob: ${employee.mobileNumber || 'N/A'}`, 27, 70, { align: 'center', maxWidth: 38 });
      const emailLines = doc.splitTextToSize(employee.email, 37);
      doc.text(emailLines, 27, 74, { align: 'center' });

      doc.setDrawColor(190, 24, 93);
      doc.setLineWidth(0.4);
      doc.line(10, 81, 44, 81);
      doc.setFontSize(6.5);
      doc.setTextColor(107, 114, 128);
      doc.text('Member of creative events operations', 27, 83.5, { align: 'center' });

      const safeName = employee.fullName.replace(/\s+/g, '-');
      doc.save(`ATAV-ID-Card-${safeName}.pdf`);
    } catch {
      this.toastr.error('Unable to generate ID card PDF right now.');
    } finally {
      this.isGeneratingIdCard = false;
    }
  }

  isFieldInvalid(field: string): boolean {
    const control = this.employeeForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    if (!file.type.startsWith('image/')) {
      this.toastr.error('Please upload a valid image file.', 'Invalid File');
      return;
    }

    if (file.size > 2 * 1024 * 1024) {
      this.toastr.error('Profile photo must be 2MB or smaller.', 'File Too Large');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const result = typeof reader.result === 'string' ? reader.result : '';
      this.photoPreview = result;
      this.employeeForm.patchValue({ profilePhoto: result });
      this.employeeForm.get('profilePhoto')?.markAsTouched();
    };
    reader.readAsDataURL(file);
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formValue = this.employeeForm.getRawValue();
    this.isSubmitting = true;

    this.employeeService.register({
      firstName: formValue.firstName!,
      lastName: formValue.lastName!,
      email: formValue.email!,
      mobileNumber: formValue.mobileNumber!,
      roleId: formValue.roleId!,
      joiningDate: formValue.joiningDate!,
      profilePhotoDataUrl: formValue.profilePhoto!
    }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (!response.success) {
          this.toastr.error(response.message || 'Unable to register employee.');
          return;
        }

        this.toastr.success('Employee added with Pending Onboarding status.', 'Success');
        this.closeRegisterPopup();
        this.loadEmployees();
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }

  private toDateInputValue(value: string): string {
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return date.toISOString().split('T')[0];
  }

  private loadImageAsDataUrl(url: string): Promise<string | null> {
    if (!url) {
      return Promise.resolve(null);
    }

    return fetch(url)
      .then(response => {
        if (!response.ok) {
          return null;
        }
        return response.blob();
      })
      .then(blob => {
        if (!blob) {
          return null;
        }

        return new Promise<string | null>((resolve) => {
          const reader = new FileReader();
          reader.onload = () => resolve(typeof reader.result === 'string' ? reader.result : null);
          reader.onerror = () => resolve(null);
          reader.readAsDataURL(blob);
        });
      })
      .catch(() => null);
  }

}
