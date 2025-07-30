import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {switchMap} from 'rxjs';
import {AuthService} from "../../services/auth.service";
import {Loan, LoanService} from "../../services/loan.service";
import {RouterLink} from "@angular/router";

@Component({
    selector: 'app-loan-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './loan-list.component.html'
})

export class LoanListComponent implements OnInit {
    loans: Loan[] = [];

    constructor(private authService: AuthService, private loanService: LoanService) {
    }

    ngOnInit(): void {
        this.authService.login()
            .pipe(
                switchMap(() => {
                    console.log('⚙️ Login finalizado. Fazendo chamada para getAllLoans()');
                    return this.loanService.getAllLoans();
                })
            )
            .subscribe({
              next: (data: Loan[]) => {
                console.log('✅ Loans retrieved:', data);
                this.loans = data;
              },
              error: (err) => {
                console.error('❌ Error fetching loans:', err);
              }
            });
    }
}
