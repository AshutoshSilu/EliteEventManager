import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { ApiResponse } from '../models/api-response.model';
import { DashboardKpi, MonthlySales } from '../models/payment.model';

@Injectable({ providedIn: 'root' })
export class ReportService {
  constructor(private http: HttpClient) {}

  getDashboardKpis(): Observable<ApiResponse<DashboardKpi>> {
    return this.http.get<ApiResponse<DashboardKpi>>(API_ENDPOINTS.reports.dashboardKpis);
  }

  getMonthlySales(year: number): Observable<ApiResponse<MonthlySales[]>> {
    return this.http.get<ApiResponse<MonthlySales[]>>(API_ENDPOINTS.reports.monthlySales(year));
  }

  getRevenueReport(startDate: string, endDate: string): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${API_ENDPOINTS.reports.revenue}?startDate=${startDate}&endDate=${endDate}`);
  }

  getBookingReport(startDate: string, endDate: string, status?: string): Observable<ApiResponse<any[]>> {
    let url = `${API_ENDPOINTS.reports.bookings}?startDate=${startDate}&endDate=${endDate}`;
    if (status) url += `&status=${status}`;
    return this.http.get<ApiResponse<any[]>>(url);
  }
}
