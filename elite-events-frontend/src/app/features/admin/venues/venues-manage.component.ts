import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VenueService } from '@core/services/venue.service';
import { VenueListItem } from '@core/models/venue.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-venues-manage',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page-content">
      <div class="d-flex justify-content-between align-items-center mb-4"><h2>Manage Venues</h2><button class="btn btn-primary-custom btn-sm">+ Add Venue</button></div>
      <div class="card">
        <div class="p-3 border-bottom"><input type="text" class="form-control" style="max-width:300px" placeholder="Search venues..." [(ngModel)]="searchTerm" (input)="load()"></div>
        <div class="table-responsive">
          <table class="table"><thead><tr><th>Name</th><th>Address</th><th>Capacity</th><th>Price/Day</th><th>Rating</th><th>Actions</th></tr></thead>
            <tbody>@for (v of venues(); track v.id) {
              <tr><td class="fw-medium">{{ v.name }}</td><td>{{ v.address }}</td><td>{{ v.capacity }}</td><td>{{ v.pricePerDay | currency:'INR' }}</td>
              <td>{{ v.rating }} <small class="text-muted">({{ v.totalReviews }})</small></td>
              <td><button class="btn-icon"><span class="material-icons">edit</span></button><button class="btn-icon text-danger" (click)="remove(v)"><span class="material-icons">delete</span></button></td></tr>
            } @empty { <tr><td colspan="6" class="text-center py-4 text-muted">No venues</td></tr> }</tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`h2{font-weight:700}.card{background:white;border-radius:12px;border:1px solid #e2e8f0;overflow:hidden}.table{margin:0}.table th{font-size:.8rem;text-transform:uppercase;color:#64748b;padding:12px 16px}.table td{padding:12px 16px;vertical-align:middle}.btn-icon{background:none;border:none;cursor:pointer;padding:4px;color:#64748b}.btn-icon:hover{color:#6366f1}.text-danger:hover{color:#ef4444!important}`]
})
export class VenuesManageComponent implements OnInit {
  private venueService = inject(VenueService);
  private toastr = inject(ToastrService);
  venues = signal<VenueListItem[]>([]);
  searchTerm = '';

  ngOnInit(): void { this.load(); }
  load(): void { this.venueService.getAll({ pageNumber: 1, pageSize: 50, searchTerm: this.searchTerm }).subscribe(r => { if (r.success && r.data) this.venues.set(r.data.items); }); }
  remove(v: VenueListItem): void { if (confirm(`Delete "${v.name}"?`)) this.venueService.delete(v.id).subscribe(r => { if (r.success) { this.toastr.success('Deleted'); this.load(); } }); }
}
