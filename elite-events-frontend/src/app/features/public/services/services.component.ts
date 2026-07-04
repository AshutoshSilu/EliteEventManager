import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.scss']
})
export class ServicesComponent {
  services = [
    { icon: 'celebration', title: 'Wedding Planning', description: 'Complete wedding management from concept to execution.', features: ['Venue selection', 'Vendor coordination', 'Theme & decor design', 'Day-of coordination', 'Guest management'] },
    { icon: 'business_center', title: 'Corporate Events', description: 'Professional events that elevate your brand.', features: ['Conferences & seminars', 'Product launches', 'Award ceremonies', 'Team building events', 'Gala dinners'] },
    { icon: 'cake', title: 'Social Events', description: 'Celebrations that create lasting memories.', features: ['Birthday parties', 'Anniversary celebrations', 'Baby showers', 'Graduation parties', 'Reunions'] },
    { icon: 'camera_alt', title: 'Photography & Video', description: 'Capture every moment beautifully.', features: ['Event photography', 'Cinematic videography', 'Drone coverage', 'Photo booths', 'Live streaming'] },
    { icon: 'restaurant', title: 'Catering Services', description: 'Culinary experiences for every palate.', features: ['Multi-cuisine menus', 'Custom menu planning', 'Bar & beverage service', 'Live cooking stations', 'Dietary accommodations'] },
    { icon: 'palette', title: 'Decor & Design', description: 'Transform spaces into extraordinary experiences.', features: ['Theme conceptualization', 'Floral arrangements', 'Lighting design', 'Stage setup', 'Table settings'] }
  ];
}
