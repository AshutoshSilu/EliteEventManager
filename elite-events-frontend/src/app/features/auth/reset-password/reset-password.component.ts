import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private toastr = inject(ToastrService);

  isSubmitting = false;
  private token = '';

  resetForm = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParams['token'] || '';
    if (!this.token) {
      this.toastr.error('Invalid reset link.', 'Error');
      this.router.navigate(['/auth/forgot-password']);
    }
  }

  isFieldInvalid(field: string): boolean {
    const control = this.resetForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit(): void {
    if (this.resetForm.invalid) {
      this.resetForm.markAllAsTouched();
      return;
    }

    const { newPassword, confirmPassword } = this.resetForm.value;
    if (newPassword !== confirmPassword) {
      this.toastr.error('Passwords do not match.', 'Error');
      return;
    }

    this.isSubmitting = true;
    this.authService.resetPassword(this.token, newPassword!, confirmPassword!).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (response.success) {
          this.toastr.success('Password reset successfully. Please login.', 'Success');
          this.router.navigate(['/auth/login']);
        }
      },
      error: () => { this.isSubmitting = false; }
    });
  }
}
