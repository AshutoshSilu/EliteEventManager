import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '@core/services/auth.service';
import { API_ENDPOINTS } from '@core/constants/api-endpoints';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);

  isSaving = false;

  profileForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    phoneNumber: [''],
    profileImageUrl: ['']
  });

  ngOnInit(): void {
    this.http.get<any>(API_ENDPOINTS.users.profile).subscribe(res => {
      if (res.success && res.data) {
        this.profileForm.patchValue(res.data);
      }
    });
  }

  onSave(): void {
    if (this.profileForm.invalid) return;
    this.isSaving = true;
    this.http.put<any>(API_ENDPOINTS.users.profile, this.profileForm.value).subscribe({
      next: (res) => {
        this.isSaving = false;
        if (res.success) this.toastr.success('Profile updated successfully');
      },
      error: () => { this.isSaving = false; }
    });
  }
}
