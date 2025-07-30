import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-create-loan',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './create-loan.component.html',
})
export class CreateLoanComponent {
  applicantName = '';
  amount: number | null = null;
  currentBalance: number | null = null;
  error: string | null = null;
  success: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  createLoan() {
    this.error = null;
    this.success = false;

    const loan = {
      applicantName: this.applicantName,
      amount: this.amount,
      currentBalance: this.currentBalance,
    };

    this.http.post('http://localhost:8080/loans', loan).subscribe({
      next: () => {
        this.success = true;
        this.clearForm();
      },
      error: () => {
        this.error = 'Failed to create loan. Please try again.';
      }
    });
  }

  private clearForm() {
    this.applicantName = '';
    this.amount = null;
    this.currentBalance = null;
  }
}
