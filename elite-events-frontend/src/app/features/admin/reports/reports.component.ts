import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService } from '@core/services/report.service';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page-content">
      <h2>Reports & Analytics</h2>
      <div class="row g-4 mb-4">
        <div class="col-md-6 col-lg-3">
          <div class="report-card" (click)="activeReport = 'revenue'">
            <span class="material-icons">account_balance</span><h5>Revenue Report</h5>
          </div>
        </div>
        <div class="col-md-6 col-lg-3">
          <div class="report-card" (click)="activeReport = 'bookings'">
            <span class="material-icons">book_online</span><h5>Booking Report</h5>
          </div>
        </div>
        <div class="col-md-6 col-lg-3">
          <div class="report-card" (click)="activeReport = 'monthly'">
            <span class="material-icons">trending_up</span><h5>Monthly Sales</h5>
          </div>
        </div>
        <div class="col-md-6 col-lg-3">
          <div class="report-card" (click)="activeReport = 'customers'">
            <span class="material-icons">people</span><h5>Customer Report</h5>
          </div>
        </div>
      </div>

      @if (activeReport) {
        <div class="card">
          <div class="card-body">
            <div class="filter-row">
              <div class="d-flex gap-2">
                <input type="date" class="form-control" [(ngModel)]="startDate" style="max-width:180px">
                <input type="date" class="form-control" [(ngModel)]="endDate" style="max-width:180px">
                <button class="btn btn-primary-custom btn-sm" (click)="generateReport()">Generate</button>
              </div>
              <div class="export-btns">
                <button class="btn btn-outline-custom btn-sm">Export Excel</button>
                <button class="btn btn-outline-custom btn-sm">Export PDF</button>
                <button class="btn btn-outline-custom btn-sm">Print</button>
              </div>
            </div>
            <div class="report-result mt-3">
              @if (reportData().length) {
                <table class="table"><thead><tr>
                  @for (col of getColumns(); track col) { <th>{{ col }}</th> }
                </tr></thead><tbody>
                  @for (row of reportData(); track $index) {
                    <tr>@for (col of getColumns(); track col) { <td>{{ row[col] }}</td> }</tr>
                  }
                </tbody></table>
              } @else {
                <p class="text-center text-muted py-4">Select date range and click Generate to view report</p>
              }
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    h2 { font-weight: 700; margin-bottom: 24px; }
    .report-card { background: white; border-radius: 12px; padding: 24px; text-align: center; border: 1px solid #e2e8f0; cursor: pointer; transition: all 0.2s; }
    .report-card:hover { border-color: #6366f1; box-shadow: 0 4px 12px rgba(99,102,241,0.1); transform: translateY(-2px); }
    .report-card .material-icons { font-size: 36px; color: #6366f1; margin-bottom: 8px; }
    .report-card h5 { font-weight: 600; margin: 0; font-size: 0.95rem; }
    .card { background: white; border-radius: 12px; border: 1px solid #e2e8f0; }
    .card-body { padding: 24px; }
    .filter-row { display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 12px; }
    .export-btns { display: flex; gap: 8px; }
    .form-control { border-radius: 8px; border: 1.5px solid #e2e8f0; padding: 8px 14px; }
    .table th { font-size: 0.8rem; text-transform: uppercase; color: #64748b; }
  `]
})
export class ReportsComponent {
  private reportService = inject(ReportService);
  activeReport: string | null = null;
  startDate = '';
  endDate = '';
  reportData = signal<any[]>([]);

  generateReport(): void {
    if (!this.startDate || !this.endDate) return;
    if (this.activeReport === 'revenue') {
      this.reportService.getRevenueReport(this.startDate, this.endDate).subscribe(res => {
        if (res.success && res.data) this.reportData.set(res.data);
      });
    } else if (this.activeReport === 'bookings') {
      this.reportService.getBookingReport(this.startDate, this.endDate).subscribe(res => {
        if (res.success && res.data) this.reportData.set(res.data);
      });
    }
  }

  getColumns(): string[] {
    const data = this.reportData();
    return data.length > 0 ? Object.keys(data[0]) : [];
  }
}
