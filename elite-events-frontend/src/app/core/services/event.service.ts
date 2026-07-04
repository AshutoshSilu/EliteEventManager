import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { ApiResponse, PagedResult, QueryParameters } from '../models/api-response.model';
import { Event, EventCategory, EventListItem } from '../models/event.model';

@Injectable({ providedIn: 'root' })
export class EventService {
  constructor(private http: HttpClient) {}

  getAll(params: QueryParameters): Observable<ApiResponse<PagedResult<EventListItem>>> {
    const httpParams = this.buildParams(params);
    return this.http.get<ApiResponse<PagedResult<EventListItem>>>(API_ENDPOINTS.events.base, { params: httpParams });
  }

  getById(id: number): Observable<ApiResponse<Event>> {
    return this.http.get<ApiResponse<Event>>(API_ENDPOINTS.events.byId(id));
  }

  getFeatured(count = 6): Observable<ApiResponse<EventListItem[]>> {
    return this.http.get<ApiResponse<EventListItem[]>>(`${API_ENDPOINTS.events.featured}?count=${count}`);
  }

  getUpcoming(count = 10): Observable<ApiResponse<EventListItem[]>> {
    return this.http.get<ApiResponse<EventListItem[]>>(`${API_ENDPOINTS.events.upcoming}?count=${count}`);
  }

  getByCategory(categoryId: number): Observable<ApiResponse<EventListItem[]>> {
    return this.http.get<ApiResponse<EventListItem[]>>(API_ENDPOINTS.events.byCategory(categoryId));
  }

  getCategories(): Observable<ApiResponse<EventCategory[]>> {
    return this.http.get<ApiResponse<EventCategory[]>>(API_ENDPOINTS.events.categories);
  }

  search(term: string, params: QueryParameters): Observable<ApiResponse<PagedResult<EventListItem>>> {
    const httpParams = this.buildParams(params).set('q', term);
    return this.http.get<ApiResponse<PagedResult<EventListItem>>>(API_ENDPOINTS.events.search, { params: httpParams });
  }

  create(event: any): Observable<ApiResponse<Event>> {
    return this.http.post<ApiResponse<Event>>(API_ENDPOINTS.events.base, event);
  }

  update(id: number, event: any): Observable<ApiResponse<Event>> {
    return this.http.put<ApiResponse<Event>>(API_ENDPOINTS.events.byId(id), event);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(API_ENDPOINTS.events.byId(id));
  }

  private buildParams(params: QueryParameters): HttpParams {
    let httpParams = new HttpParams();
    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortDirection) httpParams = httpParams.set('sortDirection', params.sortDirection);
    if (params.filterBy) httpParams = httpParams.set('filterBy', params.filterBy);
    if (params.filterValue) httpParams = httpParams.set('filterValue', params.filterValue);
    return httpParams;
  }
}
