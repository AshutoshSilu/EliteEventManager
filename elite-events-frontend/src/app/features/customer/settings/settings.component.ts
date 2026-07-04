import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '@core/services/auth.service';
import { ThemeService } from '@core/services/theme.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
  themeService = inject(ThemeService);
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);

  passwordForm = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required]
  });

  changePassword(): void {
    const { currentPassword, newPassword, confirmPassword } = this.passwordForm.value;
    if (newPassword !== confirmPassword) {
      this.toastr.error('Passwords do not match');
      return;
    }
    this.authService.changePassword(currentPassword!, newPassword!, confirmPassword!).subscribe(res => {
      if (res.success) {
        this.toastr.success('Password updated successfully');
        this.passwordForm.reset();
      }
    });
  }
}
