import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { API_ENDPOINTS } from '@core/constants/api-endpoints';
import { User } from '@core/models/user.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page-content">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Manage Users</h2>
        <button class="btn btn-primary-custom btn-sm">+ Add User</button>
      </div>
      <div class="card">
        <div class="card-header-filters">
          <input type="text" class="form-control" placeholder="Search users..." [(ngModel)]="searchTerm" (input)="loadUsers()" style="max-width:300px">
          <select class="form-select" [(ngModel)]="roleFilter" (change)="loadUsers()" style="max-width:160px">
            <option value="">All Roles</option>
            <option value="1">Administrator</option>
            <option value="2">Event Manager</option>
            <option value="3">Customer</option>
            <option value="4">Vendor</option>
          </select>
        </div>
        <div class="table-responsive">
          <table class="table">
            <thead><tr><th>Name</th><th>Email</th><th>Role</th><th>Status</th><th>Joined</th><th>Actions</th></tr></thead>
            <tbody>
              @for (user of users(); track user.id) {
                <tr>
                  <td><div class="user-cell"><div class="avatar-sm">{{ user.firstName?.charAt(0) }}</div><span>{{ user.fullName }}</span></div></td>
                  <td>{{ user.email }}</td>
                  <td><span class="role-badge">{{ user.roleName }}</span></td>
                  <td><span class="status-dot" [class.active]="user.isActive"></span> {{ user.isActive ? 'Active' : 'Inactive' }}</td>
                  <td>{{ user.createdAt | date:'mediumDate' }}</td>
                  <td>
                    <button class="btn-icon" title="Edit"><span class="material-icons">edit</span></button>
                    <button class="btn-icon text-danger" title="Delete" (click)="deleteUser(user)"><span class="material-icons">delete</span></button>
                  </td>
                </tr>
              } @empty {
                <tr><td colspan="6" class="text-center py-4 text-muted">No users found</td></tr>
              }
            </tbody>
          </table>
        </div>
        <div class="card-footer-pagination">
          <span class="text-muted">Showing {{ users().length }} users</span>
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
    .user-cell { display: flex; align-items: center; gap: 10px; }
    .avatar-sm { width: 32px; height: 32px; border-radius: 50%; background: #6366f1; color: white; display: flex; align-items: center; justify-content: center; font-size: 0.8rem; font-weight: 700; }
    .role-badge { background: #f1f5f9; padding: 4px 10px; border-radius: 12px; font-size: 0.75rem; font-weight: 600; color: #475569; }
    .status-dot { width: 8px; height: 8px; border-radius: 50%; display: inline-block; margin-right: 6px; background: #cbd5e1; }
    .status-dot.active { background: #10b981; }
    .btn-icon { background: none; border: none; cursor: pointer; padding: 4px; color: #64748b; }
    .btn-icon:hover { color: #6366f1; }
    .btn-icon.text-danger:hover { color: #ef4444; }
    .card-footer-pagination { padding: 12px 20px; border-top: 1px solid #f1f5f9; }
  `]
})
export class UsersListComponent implements OnInit {
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);

  users = signal<User[]>([]);
  searchTerm = '';
  roleFilter = '';

  ngOnInit(): void { this.loadUsers(); }

  loadUsers(): void {
    let params = new HttpParams().set('pageSize', '50');
    if (this.searchTerm) params = params.set('searchTerm', this.searchTerm);
    this.http.get<any>(API_ENDPOINTS.users.base, { params }).subscribe(res => {
      if (res.success && res.data) this.users.set(res.data.items || res.data);
    });
  }

  deleteUser(user: User): void {
    if (confirm(`Delete user ${user.fullName}?`)) {
      this.http.delete<any>(`${API_ENDPOINTS.users.base}/${user.id}`).subscribe(res => {
        if (res.success) { this.toastr.success('User deleted'); this.loadUsers(); }
      });
    }
  }
}
