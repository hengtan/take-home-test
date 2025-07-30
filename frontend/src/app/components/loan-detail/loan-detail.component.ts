import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-loan-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './loan-detail.component.html',
})
export class LoanDetailComponent {
  loanId: string = '';
  loan: any = null;
  error: string | null = null;

  constructor(private http: HttpClient) {}

  fetchLoan() {
    this.loan = null;
    this.error = null;

    this.http.get<any>(`http://localhost:8080/loans/${this.loanId}`).subscribe({
      next: (res) => (this.loan = res),
      error: (err) => (this.error = 'Loan not found or invalid ID.'),
    });
  }
}
