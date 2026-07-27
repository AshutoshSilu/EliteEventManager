import { Component, inject, OnInit, OnDestroy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { EventService } from '@core/services/event.service';
import { VenueService } from '@core/services/venue.service';
import { EventListItem } from '@core/models/event.model';
import { VenueListItem } from '@core/models/venue.model';
import { resolveImageUrl } from '@core/utils/image-url.util';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  private eventService = inject(EventService);
  private venueService = inject(VenueService);
  private slideInterval: any;
  readonly resolveImageUrl = resolveImageUrl;

  featuredEvents = signal<EventListItem[]>([]);
  featuredVenues = signal<VenueListItem[]>([]);
  currentSlide = 0;

  heroSlides = [
    { url: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=1920&h=1080&fit=crop' },
    { url: 'https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?w=1920&h=1080&fit=crop' },
    { url: 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1920&h=1080&fit=crop' },
    { url: 'https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=1920&h=1080&fit=crop' },
    { url: 'https://images.unsplash.com/photo-1492684223066-81342ee5ff30?w=1920&h=1080&fit=crop' }
  ];

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
    this.startSlideshow();
  }

  ngOnDestroy(): void {
    this.stopSlideshow();
  }

  goToSlide(index: number): void {
    this.currentSlide = index;
    this.restartSlideshow();
  }

  private startSlideshow(): void {
    this.slideInterval = setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % this.heroSlides.length;
    }, 5000);
  }

  private stopSlideshow(): void {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
    }
  }

  private restartSlideshow(): void {
    this.stopSlideshow();
    this.startSlideshow();
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
