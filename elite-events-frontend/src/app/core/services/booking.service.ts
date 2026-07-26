import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { ApiResponse, PagedResult, QueryParameters } from '../models/api-response.model';
import { Booking, BookingCreateRequest, BookingListItem } from '../models/booking.model';

@Injectable({ providedIn: 'root' })
export class BookingService {
  constructor(private http: HttpClient) {}

  getAll(params: QueryParameters): Observable<ApiResponse<PagedResult<BookingListItem>>> {
    let httpParams = new HttpParams();
    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.filterBy) httpParams = httpParams.set('filterBy', params.filterBy);
    if (params.filterValue) httpParams = httpParams.set('filterValue', params.filterValue);
    return this.http.get<ApiResponse<PagedResult<BookingListItem>>>(API_ENDPOINTS.bookings.base, { params: httpParams });
  }

  getById(id: number): Observable<ApiResponse<Booking>> {
    return this.http.get<ApiResponse<Booking>>(`${API_ENDPOINTS.bookings.base}/${id}`);
  }

  getByNumber(bookingNumber: string): Observable<ApiResponse<Booking>> {
    return this.http.get<ApiResponse<Booking>>(API_ENDPOINTS.bookings.byNumber(bookingNumber));
  }

  getMyBookings(): Observable<ApiResponse<BookingListItem[]>> {
    return this.http.get<ApiResponse<BookingListItem[]>>(API_ENDPOINTS.bookings.myBookings);
  }

  create(booking: BookingCreateRequest): Observable<ApiResponse<Booking>> {
    return this.http.post<ApiResponse<Booking>>(API_ENDPOINTS.bookings.base, booking);
  }

  updateStatus(id: number, status: string, notes?: string, cancelReason?: string): Observable<ApiResponse<Booking>> {
    return this.http.patch<ApiResponse<Booking>>(`${API_ENDPOINTS.bookings.base}/${id}/customer-action`, { status, notes, cancelReason });
  }

  cancel(id: number, reason: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(API_ENDPOINTS.bookings.cancel(id), JSON.stringify(reason), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  getTodaysCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(API_ENDPOINTS.bookings.todayCount);
  }
}
