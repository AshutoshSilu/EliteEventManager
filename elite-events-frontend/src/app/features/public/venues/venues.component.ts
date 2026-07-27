import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { VenueService } from '@core/services/venue.service';
import { VenueListItem } from '@core/models/venue.model';
import { resolveImageUrl } from '@core/utils/image-url.util';

@Component({
  selector: 'app-venues',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.scss']
})
export class VenuesComponent implements OnInit {
  private venueService = inject(VenueService);
  readonly resolveImageUrl = resolveImageUrl;
  venues = signal<VenueListItem[]>([]);
  searchTerm = '';
  sortBy = '';

  ngOnInit(): void { this.loadVenues(); }

  loadVenues(): void {
    this.venueService.getAll({ pageNumber: 1, pageSize: 12, searchTerm: this.searchTerm, sortBy: this.sortBy })
      .subscribe(res => { if (res.success && res.data) this.venues.set(res.data.items); });
  }
}
