import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HeaderComponent, FooterComponent],
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent {
  private fb = inject(FormBuilder);
  private toastr = inject(ToastrService);

  contactForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    subject: ['', Validators.required],
    message: ['', Validators.required]
  });

  onSubmit(): void {
    if (this.contactForm.valid) {
      this.toastr.success('Your message has been sent. We will get back to you soon!', 'Message Sent');
      this.contactForm.reset();
    }
  }
}
