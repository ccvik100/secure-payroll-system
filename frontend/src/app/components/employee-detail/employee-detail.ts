import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ApiService, Employee, Compensation } from '../../services/api';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { CompensationFormComponent } from '../compensation-form/compensation-form';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule, MatProgressSpinnerModule],
  templateUrl: './employee-detail.html'
})
export class EmployeeDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private apiService = inject(ApiService);

  private dialog = inject(MatDialog);

  employee = signal<Employee | null>(null);
  compensation = signal<Compensation | null>(null);
  loading = signal<boolean>(true);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id) {
      this.apiService.getEmployee(id).subscribe({
        next: (empData) => {
          this.employee.set(empData);
          
          this.apiService.getCompensation(id).subscribe({
            next: (compData) => {
              this.compensation.set(compData);
              this.loading.set(false);
            },
            error: () => {
              this.loading.set(false);
            }
          });
        },
        error: () => {
          this.loading.set(false);
        }
      });
    }
  }

  openCompensationForm() {
    const empId = this.route.snapshot.paramMap.get('id');
    if (!empId) return;

    const currentComp = this.compensation();

    const dialogRef = this.dialog.open(CompensationFormComponent, {
      width: '400px',
      data: { employeeId: empId, compensation: currentComp }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loading.set(true);
        if (currentComp && currentComp.id) {
          this.apiService.updateCompensation(currentComp.id, result).subscribe(() => this.reloadCompensation(empId));
        } else {
          this.apiService.createCompensation(result).subscribe(() => this.reloadCompensation(empId));
        }
      }
    });
  }

  reloadCompensation(empId: string) {
    this.apiService.getCompensation(empId).subscribe({
      next: (data) => {
        this.compensation.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
  
}