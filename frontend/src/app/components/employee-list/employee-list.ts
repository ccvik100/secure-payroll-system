import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, Employee } from '../../services/api';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EmployeeFormComponent } from '../employee-form/employee-form';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatButtonModule, RouterModule, MatDialogModule],
  templateUrl: './employee-list.html'
})
export class EmployeeListComponent implements OnInit {
  private apiService = inject(ApiService);
  private dialog = inject(MatDialog);
  
  displayedColumns: string[] = ['name', 'email', 'department', 'position', 'actions'];
  dataSource = new MatTableDataSource<Employee>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees() {
    this.apiService.getEmployees().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
    });
  }

  openForm(employee?: Employee) {
    const dialogRef = this.dialog.open(EmployeeFormComponent, {
      width: '400px',
      data: employee || null
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (employee && employee.id) {
          this.apiService.updateEmployee(employee.id, result).subscribe(() => this.loadEmployees());
        } else {
          this.apiService.createEmployee(result).subscribe(() => this.loadEmployees());
        }
      }
    });
  }

  deleteEmployee(id: string) {
    if (confirm('Biztosan törölni szeretnéd ezt a dolgozót?')) {
      this.apiService.deleteEmployee(id).subscribe(() => this.loadEmployees());
    }
  }
}