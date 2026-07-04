import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { VenueService } from '@core/services/venue.service';
import { Venue } from '@core/models/venue.model';

@Component({
  selector: 'app-venue-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './venue-detail.component.html',
  styleUrls: ['./venue-detail.component.scss']
})
export class VenueDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private venueService = inject(VenueService);
  venue = signal<Venue | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.params['id']);
    this.venueService.getById(id).subscribe(res => { if (res.success && res.data) this.venue.set(res.data); });
  }
}
