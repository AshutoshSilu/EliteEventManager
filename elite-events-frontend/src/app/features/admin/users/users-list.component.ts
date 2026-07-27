import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EmployeeListItem, EmployeeStatus } from '@core/models/employee.model';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '@core/services/employee.service';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="page-content">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Employee Management</h2>
        <a routerLink="/customer/employee-registration" class="btn btn-primary-custom btn-sm">+ Add Employee</a>
      </div>
      <div class="card">
        <div class="card-header-filters">
          <input type="text" class="form-control" placeholder="Search employees..." [(ngModel)]="searchTerm" (input)="loadUsers()" style="max-width:300px">
          <select class="form-select" [(ngModel)]="statusFilter" (change)="applyFilters()" style="max-width:220px">
            <option value="">All Statuses</option>
            <option value="Pending Onboarding">Pending Onboarding</option>
            <option value="Onboarded">Onboarded</option>
            <option value="Resigned">Resigned</option>
            <option value="Terminated">Terminated</option>
          </select>
        </div>
        <div class="table-responsive">
          <table class="table">
            <thead><tr><th>Profile</th><th>Full Name</th><th>Email</th><th>Mobile Number</th><th>Role</th><th>Joining Date</th><th>Current Status</th><th>Actions</th></tr></thead>
            <tbody>
              @for (employee of filteredEmployees(); track employee.userId) {
                <tr>
                  <td>
                    @if (employee.profilePhotoUrl) {
                      <img class="avatar-img" [src]="employee.profilePhotoUrl" alt="Profile photo">
                    } @else {
                      <div class="avatar-sm">{{ employee.firstName.charAt(0) }}</div>
                    }
                  </td>
                  <td>{{ employee.fullName }}</td>
                  <td>{{ employee.email }}</td>
                  <td>{{ employee.mobileNumber || '—' }}</td>
                  <td><span class="role-badge">{{ employee.roleName }}</span></td>
                  <td>{{ employee.joiningDate | date:'mediumDate' }}</td>
                  <td>
                    <span class="status-pill" [ngClass]="statusClass(employee.currentStatus)">{{ employee.currentStatus }}</span>
                  </td>
                  <td>
                    @if (canShowAction(employee.currentStatus, 'onboard')) {
                      <button class="btn btn-sm btn-success me-2" (click)="onboardEmployee(employee)">Onboard Employee</button>
                    }
                    @if (canShowAction(employee.currentStatus, 'resign')) {
                      <button class="btn btn-sm btn-warning me-2" (click)="resignEmployee(employee)">Resign</button>
                    }
                    @if (canShowAction(employee.currentStatus, 'terminate')) {
                      <button class="btn btn-sm btn-danger" (click)="terminateEmployee(employee)">Terminate</button>
                    }
                  </td>
                </tr>
              } @empty {
                <tr><td colspan="8" class="text-center py-4 text-muted">No employees found</td></tr>
              }
            </tbody>
          </table>
        </div>
        <div class="card-footer-pagination">
          <span class="text-muted">Showing {{ filteredEmployees().length }} employees</span>
        </div>
      </div>
    </div>
  `,
  styles: [`
    h2 { font-weight: 700; }
    .card { background: white; border-radius: 12px; border: 1px solid #e2e8f0; overflow: hidden; }
    .card-header-filters { display: flex; gap: 12px; padding: 16px 20px; border-bottom: 1px solid #f1f5f9; flex-wrap: wrap; }
    .form-control, .form-select { border-radius: 8px; border: 1.5px solid #e2e8f0; padding: 8px 14px; font-size: 0.9rem; }
    .table { margin: 0; }
    .table th { font-size: 0.8rem; text-transform: uppercase; color: #64748b; font-weight: 600; padding: 12px 16px; }
    .table td { padding: 12px 16px; vertical-align: middle; }
    .avatar-sm { width: 32px; height: 32px; border-radius: 50%; background: #6366f1; color: white; display: flex; align-items: center; justify-content: center; font-size: 0.8rem; font-weight: 700; }
    .avatar-img { width: 32px; height: 32px; border-radius: 50%; object-fit: cover; border: 1px solid #dbe2ef; }
    .role-badge { background: #f1f5f9; padding: 4px 10px; border-radius: 12px; font-size: 0.75rem; font-weight: 600; color: #475569; }
    .status-pill { display: inline-block; padding: 4px 10px; border-radius: 999px; font-size: 0.75rem; font-weight: 600; }
    .status-pending { background: #fef3c7; color: #92400e; }
    .status-onboarded { background: #dcfce7; color: #166534; }
    .status-resigned { background: #e2e8f0; color: #334155; }
    .status-terminated { background: #fee2e2; color: #991b1b; }
    .card-footer-pagination { padding: 12px 20px; border-top: 1px solid #f1f5f9; }
  `]
})
export class UsersListComponent implements OnInit {
  private employeeService = inject(EmployeeService);
  private toastr = inject(ToastrService);

  employees = signal<EmployeeListItem[]>([]);
  filteredEmployees = signal<EmployeeListItem[]>([]);
  searchTerm = '';
  statusFilter = '';

  ngOnInit(): void { this.loadUsers(); }

  loadUsers(): void {
    this.employeeService.getAll({
      pageSize: 300,
      searchTerm: this.searchTerm || undefined,
      sortBy: 'joiningDate',
      sortDirection: 'desc'
    }).subscribe(res => {
      if (res.success && res.data) {
        this.employees.set(res.data.items || []);
        this.applyFilters();
      }
    });
  }

  applyFilters(): void {
    const status = this.statusFilter;
    if (!status) {
      this.filteredEmployees.set(this.employees());
      return;
    }

    this.filteredEmployees.set(this.employees().filter(e => e.currentStatus === status));
  }

  statusClass(status: EmployeeStatus): string {
    if (status === 'Pending Onboarding') return 'status-pending';
    if (status === 'Onboarded') return 'status-onboarded';
    if (status === 'Resigned') return 'status-resigned';
    return 'status-terminated';
  }

  canShowAction(status: EmployeeStatus, action: 'onboard' | 'resign' | 'terminate'): boolean {
    if (status === 'Resigned' || status === 'Terminated') {
      return false;
    }

    if (action === 'onboard') {
      return status === 'Pending Onboarding';
    }

    if (action === 'resign') {
      return status === 'Onboarded';
    }

    return status === 'Pending Onboarding' || status === 'Onboarded';
  }

  onboardEmployee(employee: EmployeeListItem): void {
    this.employeeService.onboard(employee.userId).subscribe(res => {
      if (res.success) {
        this.toastr.success(`${employee.fullName} onboarded successfully.`);
        this.loadUsers();
      }
    });
  }

  resignEmployee(employee: EmployeeListItem): void {
    this.employeeService.resign(employee.userId).subscribe(res => {
      if (res.success) {
        this.toastr.info(`${employee.fullName} marked as resigned.`);
        this.loadUsers();
      }
    });
  }

  terminateEmployee(employee: EmployeeListItem): void {
    this.employeeService.terminate(employee.userId).subscribe(res => {
      if (res.success) {
        this.toastr.warning(`${employee.fullName} marked as terminated.`);
        this.loadUsers();
      }
    });
  }
}
