import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { LoanService } from './services/loan.service';
import { Loan } from './models/loan.model';
import { PaginatedResponse } from './models/paginated-response.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatPaginatorModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = [
    'loanAmount',
    'currentBalance',
    'applicant',
    'status',
  ];
  loans: any[] = [];
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private readonly loanService: LoanService) {}

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.loanService.getLoans(this.pageIndex + 1, this.pageSize).subscribe({
      next: (data: PaginatedResponse<Loan>) => {
        this.loans = data.items.map((loan: Loan) => ({
          id: loan.id,
          loanAmount: loan.amount,
          currentBalance: loan.currentBalance,
          applicant: loan.applicantName,
          status: loan.status === 0 ? 'active' : 'paid',
        }));
        this.totalItems = data.totalCount;
      },
      error: (err) => console.error('Error on fetching loans:', err),
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadLoans();
  }
}
