import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan } from '../models/loan.model';
import { PaginatedResponse } from '../models/paginates-response.model';

@Injectable({
  providedIn: 'root',
})
export class LoanService {
  private readonly baseUrl = 'http://localhost:65166/loan';

  constructor(private readonly http: HttpClient) {}

  getLoans(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PaginatedResponse<Loan>> {
    return this.http.get<PaginatedResponse<Loan>>(
      `${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }
}
