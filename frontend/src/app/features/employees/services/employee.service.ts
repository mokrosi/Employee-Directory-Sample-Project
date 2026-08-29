import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  Department,
  Employee,
  EmployeeDepartmentHistory,
  HistoryRecord,
  CreateEmployeeCommand,
  UpdateEmployeeCommand,
  TransferEmployeeCommand
} from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7214/api';

  // Search & Get Queries
  searchEmployees(searchTerm: string = ''): Observable<Employee[]> {
    let params = new HttpParams();
    if (searchTerm.trim()) {
      params = params.set('search', searchTerm.trim());
      params = params.set('searchTerm', searchTerm.trim());
    }
    return this.http.get<any>(`${this.apiUrl}/employees`, { params }).pipe(
      map(res => Array.isArray(res) ? res : (res?.items || []))
    );
  }

  getEmployeeById(id: any): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/employees/${id}`);
  }

  getEmployeeHistory(employeeId: any): Observable<EmployeeDepartmentHistory[]> {
    return this.http.get<EmployeeDepartmentHistory[]>(`${this.apiUrl}/employees/${employeeId}/history`);
  }

  getAllHistories(): Observable<HistoryRecord[]> {
    return this.http.get<HistoryRecord[]>(`${this.apiUrl}/employees/all-history`);
  }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${this.apiUrl}/departments`);
  }

  createDepartment(command: { name: string; maxHeadcount: number }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/departments`, command);
  }

  updateDepartment(id: any, command: { name: string; maxHeadcount: number }): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/departments/${id}`, command);
  }

  // Commands
  createEmployee(command: CreateEmployeeCommand): Observable<Employee> {
    return this.http.post<Employee>(`${this.apiUrl}/employees`, command);
  }

  updateEmployee(id: any, command: UpdateEmployeeCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/employees/${id}`, command);
  }

  transferEmployee(command: TransferEmployeeCommand): Observable<void> {
    const deptId = command.newDepartmentId || command.targetDepartmentId;
    const payload = {
      employeeId: command.employeeId,
      newDepartmentId: deptId,
      targetDepartmentId: deptId
    };
    return this.http.post<void>(`${this.apiUrl}/employees/transfer`, payload);
  }
}
