import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-packages',
  standalone: true,
  imports: [CommonModule, RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './packages.component.html',
  styleUrls: ['./packages.component.scss']
})
export class PackagesComponent {
  packages = [
    { name: 'Silver', price: 150000, isPopular: false, description: 'Perfect for intimate gatherings', features: ['Up to 100 guests', 'Basic venue setup', 'Photography (4 hours)', 'Standard catering', 'Basic decor', 'Event coordinator'] },
    { name: 'Gold', price: 350000, isPopular: true, description: 'Our most popular choice', features: ['Up to 300 guests', 'Premium venue options', 'Photography + Video (8 hours)', 'Multi-cuisine catering', 'Theme decor & lighting', 'DJ & entertainment', 'Dedicated event manager', 'Guest transportation'] },
    { name: 'Platinum', price: 750000, isPopular: false, description: 'The ultimate luxury experience', features: ['Unlimited guests', 'Luxury venue selection', 'Full photography & cinematic video', 'Gourmet catering & bar', 'Bespoke decor & floral', 'Live band + DJ', 'Complete vendor coordination', 'Valet & hospitality', 'Drone coverage'] }
  ];
}
