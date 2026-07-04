import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { EventService } from '@core/services/event.service';
import { VenueService } from '@core/services/venue.service';
import { EventListItem } from '@core/models/event.model';
import { VenueListItem } from '@core/models/venue.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private eventService = inject(EventService);
  private venueService = inject(VenueService);

  featuredEvents = signal<EventListItem[]>([]);
  featuredVenues = signal<VenueListItem[]>([]);

  services = [
    { icon: 'celebration', title: 'Wedding Planning', description: 'Complete wedding management from venue selection to the reception. Every detail handled with care.' },
    { icon: 'business_center', title: 'Corporate Events', description: 'Professional conferences, product launches, team-building events, and corporate galas.' },
    { icon: 'cake', title: 'Birthday & Parties', description: 'Themed birthday parties, anniversary celebrations, and social gatherings made special.' },
    { icon: 'location_city', title: 'Venue Booking', description: 'Access to 15+ premium venues with flexible booking options and competitive pricing.' },
    { icon: 'groups', title: 'Vendor Management', description: 'Curated network of 50+ verified vendors including photographers, caterers, and DJs.' },
    { icon: 'design_services', title: 'Event Design', description: 'Creative theme design, decor planning, and flawless execution of your vision.' }
  ];

  ngOnInit(): void {
    this.loadFeaturedEvents();
    this.loadFeaturedVenues();
  }

  private loadFeaturedEvents(): void {
    this.eventService.getFeatured(6).subscribe(res => {
      if (res.success && res.data) this.featuredEvents.set(res.data);
    });
  }

  private loadFeaturedVenues(): void {
    this.venueService.getFeatured(3).subscribe(res => {
      if (res.success && res.data) this.featuredVenues.set(res.data);
    });
  }
}
