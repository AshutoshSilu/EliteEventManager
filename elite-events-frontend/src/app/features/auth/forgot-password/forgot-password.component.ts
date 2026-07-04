import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);

  isSubmitting = false;
  emailSent = false;

  forgotForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  isFieldInvalid(field: string): boolean {
    const control = this.forgotForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit(): void {
    if (this.forgotForm.invalid) {
      this.forgotForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.authService.forgotPassword(this.forgotForm.value.email!).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.emailSent = true;
      },
      error: () => { this.isSubmitting = false; }
    });
  }
}
