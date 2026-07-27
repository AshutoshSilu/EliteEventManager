import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '@core/constants/api-endpoints';
import { ApiResponse, PagedResult, QueryParameters } from '@core/models/api-response.model';
import { CreateEmployeeRequest, EmployeeDetail, EmployeeListItem, UpdateEmployeeRequest } from '@core/models/employee.model';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private http = inject(HttpClient);

  register(request: CreateEmployeeRequest): Observable<ApiResponse<EmployeeListItem>> {
    return this.http.post<ApiResponse<EmployeeListItem>>(API_ENDPOINTS.employees.base, request);
  }

  getAll(query: QueryParameters): Observable<ApiResponse<PagedResult<EmployeeListItem>>> {
    let params = new HttpParams();
    if (query.pageNumber) params = params.set('pageNumber', query.pageNumber);
    if (query.pageSize) params = params.set('pageSize', query.pageSize);
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.sortBy) params = params.set('sortBy', query.sortBy);
    if (query.sortDirection) params = params.set('sortDirection', query.sortDirection);
    return this.http.get<ApiResponse<PagedResult<EmployeeListItem>>>(API_ENDPOINTS.employees.base, { params });
  }

  getByUserId(userId: string): Observable<ApiResponse<EmployeeDetail>> {
    return this.http.get<ApiResponse<EmployeeDetail>>(API_ENDPOINTS.employees.byUserId(userId));
  }

  update(userId: string, request: UpdateEmployeeRequest): Observable<ApiResponse<EmployeeDetail>> {
    return this.http.put<ApiResponse<EmployeeDetail>>(API_ENDPOINTS.employees.byUserId(userId), request);
  }

  onboard(userId: string): Observable<ApiResponse<EmployeeListItem>> {
    return this.http.patch<ApiResponse<EmployeeListItem>>(API_ENDPOINTS.employees.onboard(userId), {});
  }

  resign(userId: string): Observable<ApiResponse<EmployeeListItem>> {
    return this.http.patch<ApiResponse<EmployeeListItem>>(API_ENDPOINTS.employees.resign(userId), {});
  }

  terminate(userId: string): Observable<ApiResponse<EmployeeListItem>> {
    return this.http.patch<ApiResponse<EmployeeListItem>>(API_ENDPOINTS.employees.terminate(userId), {});
  }
}
