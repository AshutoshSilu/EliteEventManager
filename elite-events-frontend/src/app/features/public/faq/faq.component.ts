import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-faq',
  standalone: true,
  imports: [CommonModule, HeaderComponent, FooterComponent],
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.scss']
})
export class FaqComponent {
  openIndex: number | null = null;
  toggle(i: number): void { this.openIndex = this.openIndex === i ? null : i; }

  faqs = [
    { question: 'How do I book an event?', answer: 'Register on our platform, browse available events or packages, and click Book Now. Follow the steps to select your preferences, add services, and complete payment.' },
    { question: 'What payment methods do you accept?', answer: 'We accept UPI, Credit Cards, Debit Cards, Net Banking, and bank transfers. All payments are processed securely through our payment gateway.' },
    { question: 'Can I cancel or reschedule my booking?', answer: 'Yes, you can cancel or reschedule from your dashboard. Cancellation policies vary: full refund if 30+ days before event, 50% if 15-29 days, no refund within 14 days.' },
    { question: 'How do I become a vendor partner?', answer: 'Register as a vendor, complete your business profile with portfolio and pricing, then submit for verification. Our team reviews within 3-5 business days.' },
    { question: 'Do you offer custom packages?', answer: 'Absolutely! Contact our team to discuss your requirements and budget. We create tailored packages combining venue, catering, decor, photography, and more.' },
    { question: 'What areas do you serve?', answer: 'We primarily serve Mumbai, Pune, Bangalore, Delhi, and Chennai. We can also arrange events in other cities upon request with advance notice.' },
    { question: 'How far in advance should I book?', answer: 'We recommend booking at least 2-3 months in advance for large events and 3-4 weeks for smaller gatherings to ensure venue and vendor availability.' }
  ];
}
