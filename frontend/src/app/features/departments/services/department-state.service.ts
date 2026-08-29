import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DepartmentService } from './department.service';
import { Department, CreateDepartmentCommand, UpdateDepartmentCommand } from '../models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentStateService {
  private departmentService = inject(DepartmentService);

  private departmentsSubject = new BehaviorSubject<Department[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  departments$ = this.departmentsSubject.asObservable();
  loading$ = this.loadingSubject.asObservable();
  error$ = this.errorSubject.asObservable();

  loadDepartments(): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.departmentService.getDepartments().subscribe({
      next: (depts) => {
        this.departmentsSubject.next(depts || []);
        this.loadingSubject.next(false);
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to load departments.'));
      }
    });
  }

  createDepartment(command: CreateDepartmentCommand, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.departmentService.createDepartment(command).subscribe({
      next: () => {
        this.loadingSubject.next(false);
        this.loadDepartments();
        onSuccess();
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to create department.'));
      }
    });
  }

  updateDepartment(id: any, command: UpdateDepartmentCommand, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.departmentService.updateDepartment(id, command).subscribe({
      next: () => {
        this.loadingSubject.next(false);
        this.loadDepartments();
        onSuccess();
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to update department.'));
      }
    });
  }

  private extractErrorMessage(err: any, fallback: string): string {
    if (!err) return fallback;
    if (typeof err === 'string') return err;
    if (typeof err.error === 'string') return err.error;
    if (typeof err.error?.message === 'string') return err.error.message;
    if (typeof err.error?.detail === 'string') return err.error.detail;
    if (typeof err.message === 'string') return err.message;
    return fallback;
  }
}
