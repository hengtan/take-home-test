// src/app/services/loan.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Loan {
  id: string;
  applicantName: string;
  amount: number;
  currentBalance: number;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class LoanService {
  private baseUrl = 'http://localhost:8080/loans';

  constructor(private http: HttpClient) {}

  getAllLoans(): Observable<Loan[]> {
    return this.http.get<Loan[]>(this.baseUrl);
  }
}
