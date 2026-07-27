import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReportService } from '@core/services/report.service';
import { AuthService } from '@core/services/auth.service';
import { DashboardKpi, MonthlySales } from '@core/models/payment.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="admin-layout">
      <!-- Sidebar -->
      <aside class="admin-sidebar">
        <div class="sidebar-brand"><span>Elite<span class="accent">Admin</span></span></div>
        <nav>
          <a routerLink="/admin" class="nav-link active"><span class="material-icons">dashboard</span> Dashboard</a>
          <a routerLink="/admin/users" class="nav-link"><span class="material-icons">people</span> Users</a>
          <a routerLink="/admin/events" class="nav-link"><span class="material-icons">event</span> Events</a>
          <a routerLink="/admin/bookings" class="nav-link"><span class="material-icons">book_online</span> Bookings</a>
          <a routerLink="/admin/venues" class="nav-link"><span class="material-icons">location_city</span> Venues</a>
          <a routerLink="/admin/vendors" class="nav-link"><span class="material-icons">storefront</span> Vendors</a>
          <a routerLink="/admin/payments" class="nav-link"><span class="material-icons">payments</span> Payments</a>
          <a routerLink="/admin/gallery" class="nav-link"><span class="material-icons">photo_library</span> Gallery</a>
          <a routerLink="/admin/reviews" class="nav-link"><span class="material-icons">rate_review</span> Reviews</a>
          <a routerLink="/admin/coupons" class="nav-link"><span class="material-icons">local_offer</span> Coupons</a>
          <a routerLink="/admin/reports" class="nav-link"><span class="material-icons">analytics</span> Reports</a>
          <a routerLink="/admin/settings" class="nav-link"><span class="material-icons">settings</span> Settings</a>
        </nav>
        <div class="sidebar-footer"><a routerLink="/" class="nav-link"><span class="material-icons">home</span> Back to Site</a></div>
      </aside>

      <!-- Main -->
      <main class="admin-main">
        <header class="admin-header">
          <h2>Admin Dashboard</h2>
          <div class="header-actions">
            <span class="admin-name">{{ authService.user()?.fullName }}</span>
            <button class="btn btn-sm btn-outline-custom" (click)="authService.logout()">Logout</button>
          </div>
        </header>

        <!-- KPI Cards -->
        <div class="row g-4 mb-4">
          @for (kpi of kpiCards; track kpi.label) {
            <div class="col-xl-3 col-md-6">
              <div class="kpi-card" [style.border-left-color]="kpi.color">
                <div class="kpi-info"><span class="kpi-value">{{ getKpiValue(kpi.key) }}</span><span class="kpi-label">{{ kpi.label }}</span></div>
                <span class="material-icons kpi-icon" [style.color]="kpi.color">{{ kpi.icon }}</span>
              </div>
            </div>
          }
        </div>

        <!-- Charts Row -->
        <div class="row g-4 mb-4">
          <div class="col-lg-8">
            <div class="chart-card">
              <h5>Monthly Revenue ({{ currentYear }})</h5>
              <div class="chart-placeholder">
                <div class="bar-chart">
                  @for (m of monthlySales(); track m.monthNum) {
                    <div class="bar-wrapper" [title]="m.monthName + ': ' + m.revenue">
                      <div class="bar" [style.height.%]="getBarHeight(m.revenue)"></div>
                      <span class="bar-label">{{ m.monthName.substring(0, 3) }}</span>
                    </div>
                  }
                </div>
              </div>
            </div>
          </div>
          <div class="col-lg-4">
            <div class="chart-card">
              <h5>Quick Stats</h5>
              <div class="quick-stats">
                <div class="qs-item"><span class="material-icons text-success">trending_up</span><div><strong>{{ kpis()?.totalRevenue | currency:'INR':'symbol':'1.0-0' }}</strong><span>Total Revenue</span></div></div>
                <div class="qs-item"><span class="material-icons text-warning">pending_actions</span><div><strong>{{ kpis()?.pendingPayments | currency:'INR':'symbol':'1.0-0' }}</strong><span>Pending Payments</span></div></div>
                <div class="qs-item"><span class="material-icons text-info">event_available</span><div><strong>{{ kpis()?.upcomingEvents }}</strong><span>Upcoming Events</span></div></div>
                <div class="qs-item"><span class="material-icons text-purple">groups</span><div><strong>{{ kpis()?.activeVendors }}</strong><span>Active Vendors</span></div></div>
              </div>
            </div>
          </div>
        </div>

        <!-- Recent Activity -->
        <div class="row g-4">
          <div class="col-lg-6">
            <div class="chart-card">
              <div class="card-head"><h5>Recent Bookings</h5><a routerLink="/admin/bookings" class="link">View All</a></div>
              <p class="text-muted small">Today: {{ kpis()?.todaysBookings || 0 }} bookings</p>
              <a routerLink="/admin/employees" class="btn btn-sm btn-outline-custom mt-3">Open Employee Management</a>
            </div>
          </div>
          <div class="col-lg-6">
            <div class="chart-card">
              <div class="card-head"><h5>System Overview</h5></div>
              <div class="system-overview">
                <div class="so-row"><span>Total Users</span><strong>{{ kpis()?.totalUsers }}</strong></div>
                <div class="so-row"><span>Total Customers</span><strong>{{ kpis()?.totalCustomers }}</strong></div>
                <div class="so-row"><span>Total Bookings</span><strong>{{ kpis()?.totalBookings }}</strong></div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [`
    .admin-layout { display: flex; min-height: 100vh; }
    .admin-sidebar { width: 250px; background: #0f172a; color: #e2e8f0; padding: 20px 0; position: fixed; height: 100vh; overflow-y: auto; z-index: 100; }
    .sidebar-brand { padding: 10px 20px 30px; font-size: 1.3rem; font-weight: 700; color: white; }
    .sidebar-brand .accent { color: #6366f1; }
    .nav-link { display: flex; align-items: center; gap: 10px; padding: 11px 20px; color: #94a3b8; text-decoration: none; font-size: 0.9rem; transition: all 0.2s; }
    .nav-link:hover, .nav-link.active { background: rgba(99,102,241,0.1); color: white; border-left: 3px solid #6366f1; }
    .nav-link .material-icons { font-size: 20px; }
    .sidebar-footer { margin-top: auto; border-top: 1px solid #1e293b; padding-top: 10px; }
    .admin-main { margin-left: 250px; flex: 1; padding: 24px 32px; background: #f1f5f9; }
    .admin-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    .admin-header h2 { font-size: 1.5rem; font-weight: 700; color: #0f172a; }
    .header-actions { display: flex; align-items: center; gap: 12px; }
    .admin-name { font-weight: 500; color: #475569; }

    .kpi-card { background: white; border-radius: 12px; padding: 20px; display: flex; justify-content: space-between; align-items: center; border: 1px solid #e2e8f0; border-left: 4px solid; }
    .kpi-value { display: block; font-size: 1.8rem; font-weight: 800; color: #0f172a; }
    .kpi-label { font-size: 0.85rem; color: #64748b; }
    .kpi-icon { font-size: 40px; opacity: 0.3; }

    .chart-card { background: white; border-radius: 12px; padding: 24px; border: 1px solid #e2e8f0; height: 100%; }
    .chart-card h5 { font-weight: 600; margin-bottom: 16px; }
    .card-head { display: flex; justify-content: space-between; align-items: center; }
    .link { color: #6366f1; font-size: 0.85rem; font-weight: 500; text-decoration: none; }

    .bar-chart { display: flex; align-items: flex-end; gap: 8px; height: 200px; padding-top: 20px; }
    .bar-wrapper { flex: 1; display: flex; flex-direction: column; align-items: center; }
    .bar { width: 100%; max-width: 30px; background: linear-gradient(to top, #6366f1, #818cf8); border-radius: 4px 4px 0 0; min-height: 4px; transition: height 0.5s; }
    .bar-label { font-size: 0.7rem; color: #64748b; margin-top: 6px; }

    .quick-stats { display: flex; flex-direction: column; gap: 16px; }
    .qs-item { display: flex; align-items: center; gap: 12px; }
    .qs-item .material-icons { font-size: 28px; }
    .qs-item strong { display: block; font-size: 1.1rem; }
    .qs-item span:not(.material-icons) { font-size: 0.8rem; color: #64748b; }
    .text-success { color: #10b981; } .text-warning { color: #f59e0b; } .text-info { color: #3b82f6; } .text-purple { color: #8b5cf6; }

    .system-overview { display: flex; flex-direction: column; gap: 12px; }
    .so-row { display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #f1f5f9; }
    .so-row span { color: #64748b; }
    .so-row strong { color: #0f172a; }

    @media (max-width: 991px) { .admin-sidebar { display: none; } .admin-main { margin-left: 0; } }
  `]
})
export class AdminDashboardComponent implements OnInit {
  authService = inject(AuthService);
  private reportService = inject(ReportService);

  kpis = signal<DashboardKpi | null>(null);
  monthlySales = signal<MonthlySales[]>([]);
  currentYear = new Date().getFullYear();
  maxRevenue = 0;

  kpiCards = [
    { key: 'totalBookings', label: 'Total Bookings', icon: 'book_online', color: '#6366f1' },
    { key: 'todaysBookings', label: "Today's Bookings", icon: 'today', color: '#10b981' },
    { key: 'totalCustomers', label: 'Total Customers', icon: 'people', color: '#f59e0b' },
    { key: 'upcomingEvents', label: 'Upcoming Events', icon: 'event', color: '#3b82f6' }
  ];

  ngOnInit(): void {
    this.reportService.getDashboardKpis().subscribe(res => {
      if (res.success && res.data) this.kpis.set(res.data);
    });
    this.reportService.getMonthlySales(this.currentYear).subscribe(res => {
      if (res.success && res.data) {
        this.monthlySales.set(res.data);
        this.maxRevenue = Math.max(...res.data.map(m => m.revenue), 1);
      }
    });
  }

  getKpiValue(key: string): string | number {
    const k = this.kpis();
    if (!k) return '—';
    return (k as any)[key] ?? 0;
  }

  getBarHeight(revenue: number): number {
    return this.maxRevenue > 0 ? (revenue / this.maxRevenue) * 100 : 0;
  }
}
