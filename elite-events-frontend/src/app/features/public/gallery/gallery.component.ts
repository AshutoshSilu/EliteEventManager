import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-gallery',
  standalone: true,
  imports: [CommonModule, HeaderComponent, FooterComponent],
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent {
  activeTab = 'All';
  tabs = ['All', 'Weddings', 'Corporate', 'Birthdays', 'Decor'];

  items = [
    { url: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=600&h=400&fit=crop', title: 'Royal Wedding Setup', category: 'Weddings' },
    { url: 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=600&h=400&fit=crop', title: 'Annual Gala', category: 'Corporate' },
    { url: 'https://images.unsplash.com/photo-1530103862676-de8c9debad1d?w=600&h=400&fit=crop', title: 'Themed Birthday', category: 'Birthdays' },
    { url: 'https://images.unsplash.com/photo-1478146059778-26028b07395a?w=600&h=400&fit=crop', title: 'Floral Arrangements', category: 'Decor' },
    { url: 'https://images.unsplash.com/photo-1546032996-6dfacbacbf3f?w=600&h=400&fit=crop', title: 'Beach Wedding', category: 'Weddings' },
    { url: 'https://images.unsplash.com/photo-1505373877841-8d25f7d46678?w=600&h=400&fit=crop', title: 'Product Launch', category: 'Corporate' },
    { url: 'https://images.unsplash.com/photo-1502086223501-7ea6ecd79368?w=600&h=400&fit=crop', title: 'Kids Party', category: 'Birthdays' },
    { url: 'https://images.unsplash.com/photo-1492684223066-81342ee5ff30?w=600&h=400&fit=crop', title: 'Stage Design', category: 'Decor' },
    { url: 'https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=600&h=400&fit=crop', title: 'Grand Reception', category: 'Weddings' }
  ];

  get filteredItems() {
    return this.activeTab === 'All' ? this.items : this.items.filter(i => i.category === this.activeTab);
  }
}
