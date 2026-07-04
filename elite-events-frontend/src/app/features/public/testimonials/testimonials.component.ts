import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-testimonials',
  standalone: true,
  imports: [CommonModule, HeaderComponent, FooterComponent],
  templateUrl: './testimonials.component.html',
  styleUrls: ['./testimonials.component.scss']
})
export class TestimonialsComponent {
  testimonials = [
    { name: 'Rahul Sharma', designation: 'CEO, TechVision', rating: 5, content: 'Elite Events made our corporate gala absolutely spectacular. The attention to detail was remarkable and our guests were thoroughly impressed.' },
    { name: 'Priya Patel', designation: 'Bride', rating: 5, content: 'Our wedding was a dream come true thanks to Elite Events. Every moment was perfectly orchestrated and beautifully captured.' },
    { name: 'Amit Verma', designation: 'Director, InnovateCorp', rating: 5, content: 'Professional, creative, and reliable. They handled our product launch flawlessly. Highly recommended!' },
    { name: 'Sneha Kapoor', designation: 'Anniversary Host', rating: 4, content: 'The anniversary party they organized for my parents was beyond our expectations. Truly elite service!' },
    { name: 'Vikram Singh', designation: 'Groom', rating: 5, content: 'From venue selection to the last dance, everything was perfect. The team went above and beyond our expectations.' },
    { name: 'Meera Joshi', designation: 'HR Head, GlobalTech', rating: 5, content: 'Our annual team-building event was the best one yet. Creative activities, great food, and flawless execution.' }
  ];
}
