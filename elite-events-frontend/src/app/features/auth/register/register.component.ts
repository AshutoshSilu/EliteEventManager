import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  isSubmitting = false;

  registerForm = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: [''],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
    agreeTerms: [false, [Validators.requiredTrue]]
  }, { validators: this.passwordMatchValidator });

  isFieldInvalid(field: string): boolean {
    const control = this.registerForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  passwordMatchValidator(control: AbstractControl) {
    const password = control.get('password');
    const confirm = control.get('confirmPassword');
    if (password && confirm && password.value !== confirm.value) {
      confirm.setErrors({ mismatch: true });
    }
    return null;
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const formValue = this.registerForm.value;

    this.authService.register({
      firstName: formValue.firstName!,
      lastName: formValue.lastName!,
      email: formValue.email!,
      password: formValue.password!,
      confirmPassword: formValue.confirmPassword!,
      phoneNumber: formValue.phoneNumber || undefined
    }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (response.success) {
          this.toastr.success('Please check your email to verify your account.', 'Registration Successful');
          this.router.navigate(['/auth/login']);
        }
      },
      error: () => { this.isSubmitting = false; }
    });
  }
}
