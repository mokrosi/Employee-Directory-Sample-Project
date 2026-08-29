import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Department, CreateDepartmentCommand, UpdateDepartmentCommand } from '../models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7214/api/departments';

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(this.apiUrl);
  }

  createDepartment(command: CreateDepartmentCommand): Observable<any> {
    return this.http.post<any>(this.apiUrl, command);
  }

  updateDepartment(id: any, command: UpdateDepartmentCommand): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, command);
  }
}
