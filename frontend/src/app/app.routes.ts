import { Routes } from '@angular/router';
import { LoanListComponent } from './components/loan-list/loan-list.component';
import {HomeComponent} from './components/home/ home.component';
import {LoanDetailComponent} from './components/loan-detail/loan-detail.component';
import {CreateLoanComponent} from './components/create-loan/create-loan.component';
import {LoanPaymentComponent} from './components/loan-payment/loan-payment.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'loans', component: LoanListComponent },
  { path: 'loan-detail', component: LoanDetailComponent },
  { path: 'create-loan', component: CreateLoanComponent },
  { path: 'loan-payment', component: LoanPaymentComponent }
];
