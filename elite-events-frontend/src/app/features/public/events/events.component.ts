import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { EventService } from '@core/services/event.service';
import { EventListItem, EventCategory } from '@core/models/event.model';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {
  private eventService = inject(EventService);

  events = signal<EventListItem[]>([]);
  categories = signal<EventCategory[]>([]);
  currentPage = signal(1);
  totalPages = signal(1);
  searchTerm = '';
  selectedCategory = '';
  sortBy = '';

  ngOnInit(): void {
    this.loadCategories();
    this.loadEvents();
  }

  loadEvents(): void {
    this.eventService.getAll({
      pageNumber: this.currentPage(),
      pageSize: 9,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      filterBy: this.selectedCategory ? 'categoryId' : undefined,
      filterValue: this.selectedCategory || undefined
    }).subscribe(res => {
      if (res.success && res.data) {
        this.events.set(res.data.items);
        this.totalPages.set(res.data.totalPages);
      }
    });
  }

  loadCategories(): void {
    this.eventService.getCategories().subscribe(res => {
      if (res.success && res.data) this.categories.set(res.data);
    });
  }

  onSearch(): void { this.currentPage.set(1); this.loadEvents(); }
  onCategoryChange(): void { this.currentPage.set(1); this.loadEvents(); }
  changePage(page: number): void { this.currentPage.set(page); this.loadEvents(); }
}
