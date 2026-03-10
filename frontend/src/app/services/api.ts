import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Employee {
  id?: string;
  firstName: string;
  lastName: string;
  email: string;
  department: string;
  position: string;
}

export interface Compensation {
  id?: string;
  employeeId: string;
  taxNumber: string;
  bankAccountNumber: string;
  grossBaseSalary: number;
  currency: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);
  
  private directoryUrl = 'http://localhost:5001/api/employee';
  private vaultUrl = 'http://localhost:5002/api/compensation';

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.directoryUrl);
  }

  getEmployee(id: string): Observable<Employee> {
    return this.http.get<Employee>(`${this.directoryUrl}/${id}`);
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.directoryUrl, employee);
  }

  updateEmployee(id: string, employee: Employee): Observable<any> {
    return this.http.put(`${this.directoryUrl}/${id}`, employee);
  }

  deleteEmployee(id: string): Observable<any> {
    return this.http.delete(`${this.directoryUrl}/${id}`);
  }

  createCompensation(compensation: Compensation): Observable<Compensation> {
    return this.http.post<Compensation>(this.vaultUrl, compensation);
  }

  updateCompensation(id: string, compensation: Compensation): Observable<any> {
    return this.http.put(`${this.vaultUrl}/${id}`, compensation);
  }

  getCompensation(employeeId: string): Observable<Compensation> {
    return this.http.get<Compensation>(`${this.vaultUrl}/employee/${employeeId}`);
  }

}