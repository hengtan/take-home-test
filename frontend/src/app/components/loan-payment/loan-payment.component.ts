import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-loan-payment',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './loan-payment.component.html',
})
export class LoanPaymentComponent {
  loanId: string = '';
  amount: number | null = null;
  success = false;
  error: string | null = null;

  constructor(private http: HttpClient) {}

  makePayment() {
    this.success = false;
    this.error = null;

    const url = `http://localhost:8080/loans/${this.loanId}/payment`;

    this.http.post(url, { amount: this.amount }).subscribe({
      next: () => {
        this.success = true;
        this.loanId = '';
        this.amount = null;
      },
      error: (err) => {
        this.error = 'Payment failed. Please check the ID and try again.';
      }
    });
  }
}
