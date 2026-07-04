import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';
import { EventService } from '@core/services/event.service';
import { Event } from '@core/models/event.model';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss']
})
export class EventDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private eventService = inject(EventService);
  event = signal<Event | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.params['id']);
    this.eventService.getById(id).subscribe(res => {
      if (res.success && res.data) this.event.set(res.data);
    });
  }

  getStatusColor(status: string): string {
    const map: Record<string, string> = { Published: 'success', Draft: 'secondary', Ongoing: 'warning', Completed: 'info', Cancelled: 'danger' };
    return map[status] || 'secondary';
  }
}
