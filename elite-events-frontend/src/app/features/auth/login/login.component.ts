import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private toastr = inject(ToastrService);

  showPassword = false;
  isSubmitting = false;

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  isFieldInvalid(field: string): boolean {
    const control = this.loginForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const { email, password } = this.loginForm.value;

    this.authService.login({ email: email!, password: password! }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        if (response.success) {
          this.toastr.success('Welcome back!', 'Login Successful');
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || this.getDefaultRoute();
          this.router.navigateByUrl(returnUrl);
        }
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }

  private getDefaultRoute(): string {
    const role = this.authService.userRole();
    if (role === 'Administrator' || role === 'EventManager') return '/admin';
    return '/customer';
  }
}
