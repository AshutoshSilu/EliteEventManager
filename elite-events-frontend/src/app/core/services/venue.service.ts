import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { ApiResponse, PagedResult, QueryParameters } from '../models/api-response.model';
import { Venue, VenueListItem } from '../models/venue.model';

@Injectable({ providedIn: 'root' })
export class VenueService {
  constructor(private http: HttpClient) {}

  getAll(params: QueryParameters): Observable<ApiResponse<PagedResult<VenueListItem>>> {
    let httpParams = new HttpParams();
    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    return this.http.get<ApiResponse<PagedResult<VenueListItem>>>(API_ENDPOINTS.venues.base, { params: httpParams });
  }

  getById(id: number): Observable<ApiResponse<Venue>> {
    return this.http.get<ApiResponse<Venue>>(API_ENDPOINTS.venues.byId(id));
  }

  getFeatured(count = 6): Observable<ApiResponse<VenueListItem[]>> {
    return this.http.get<ApiResponse<VenueListItem[]>>(`${API_ENDPOINTS.venues.featured}?count=${count}`);
  }

  checkAvailability(venueId: number, date: string): Observable<ApiResponse<boolean>> {
    return this.http.get<ApiResponse<boolean>>(`${API_ENDPOINTS.venues.availability(venueId)}?date=${date}`);
  }

  create(venue: any): Observable<ApiResponse<Venue>> {
    return this.http.post<ApiResponse<Venue>>(API_ENDPOINTS.venues.base, venue);
  }

  update(id: number, venue: any): Observable<ApiResponse<Venue>> {
    return this.http.put<ApiResponse<Venue>>(API_ENDPOINTS.venues.byId(id), venue);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(API_ENDPOINTS.venues.byId(id));
  }
}
