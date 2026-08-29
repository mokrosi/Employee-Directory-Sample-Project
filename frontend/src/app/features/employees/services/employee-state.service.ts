import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Subject, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, tap, catchError } from 'rxjs/operators';
import { EmployeeService } from './employee.service';
import {
  CreateEmployeeCommand,
  Department,
  Employee,
  EmployeeDepartmentHistory,
  HistoryRecord,
  TransferEmployeeCommand,
  UpdateEmployeeCommand
} from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeStateService {
  private employeeService = inject(EmployeeService);

  private employeesSubject = new BehaviorSubject<Employee[]>([]);
  private selectedEmployeeSubject = new BehaviorSubject<Employee | null>(null);
  private historySubject = new BehaviorSubject<EmployeeDepartmentHistory[]>([]);
  private allHistoriesSubject = new BehaviorSubject<HistoryRecord[]>([]);
  private departmentsSubject = new BehaviorSubject<Department[]>([]);

  // Use Subject instead of BehaviorSubject to avoid auto-triggering on startup
  private searchSubject = new Subject<string>();

  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  employees$ = this.employeesSubject.asObservable();
  selectedEmployee$ = this.selectedEmployeeSubject.asObservable();
  history$ = this.historySubject.asObservable();
  allHistories$ = this.allHistoriesSubject.asObservable();
  departments$ = this.departmentsSubject.asObservable();
  loading$ = this.loadingSubject.asObservable();
  error$ = this.errorSubject.asObservable();

  constructor() {
    this.setupSearchStream();
  }

  private setupSearchStream(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.loadingSubject.next(true)),
      switchMap(term =>
        this.employeeService.searchEmployees(term).pipe(
          catchError(() => {
            this.errorSubject.next('Failed to search employees.');
            return of([]);
          })
        )
      )
    ).subscribe(employees => {
      this.employeesSubject.next(employees);
      this.loadingSubject.next(false);
    });
  }

  setSearchTerm(term: string): void {
    this.searchSubject.next(term);
  }

  setInitialEmployees(employees: Employee[]): void {
    this.employeesSubject.next(employees);
  }

  loadDepartments(): void {
    this.employeeService.getDepartments().subscribe({
      next: (depts) => this.departmentsSubject.next(depts),
      error: () => this.errorSubject.next('Failed to retrieve department list.')
    });
  }

  selectEmployee(employee: Employee | null): void {
    this.selectedEmployeeSubject.next(employee);
    if (employee) {
      this.loadEmployeeHistory(employee.id);
    } else {
      this.historySubject.next([]);
    }
  }

  loadEmployeeHistory(employeeId: any): void {
    this.employeeService.getEmployeeHistory(employeeId).subscribe({
      next: (history) => this.historySubject.next(history),
      error: () => this.errorSubject.next('Failed to retrieve assignment history.')
    });
  }

  createEmployee(command: CreateEmployeeCommand, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.employeeService.createEmployee(command).subscribe({
      next: () => {
        this.loadingSubject.next(false);
        this.loadDepartments();
        this.refreshEmployees();
        onSuccess();
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to create employee.'));
      }
    });
  }

  updateEmployee(id: any, command: UpdateEmployeeCommand, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.employeeService.updateEmployee(id, command).subscribe({
      next: () => {
        this.loadingSubject.next(false);
        this.refreshEmployees();
        if (this.selectedEmployeeSubject.value?.id === id) {
          this.employeeService.getEmployeeById(id).subscribe(emp => this.selectedEmployeeSubject.next(emp));
        }
        onSuccess();
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to update employee.'));
      }
    });
  }

  transferEmployee(command: TransferEmployeeCommand, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.employeeService.transferEmployee(command).subscribe({
      next: () => {
        this.loadingSubject.next(false);
        this.loadDepartments();
        this.refreshEmployees();
        if (command.employeeId) {
          this.loadEmployeeHistory(command.employeeId);
          this.employeeService.getEmployeeById(command.employeeId).subscribe(emp => this.selectedEmployeeSubject.next(emp));
        }
        onSuccess();
      },
      error: (err) => {
        this.loadingSubject.next(false);
        const msg = this.extractErrorMessage(err, 'Transfer failed.');
        if (msg.toLowerCase().includes('capacity')) {
          this.errorSubject.next('Target department is already at maximum capacity.');
        } else {
          this.errorSubject.next(msg);
        }
      }
    });
  }

  loadAllHistories(): void {
    this.loadingSubject.next(true);
    this.employeeService.getAllHistories().subscribe({
      next: (histories) => {
        this.allHistoriesSubject.next(histories);
        this.loadingSubject.next(false);
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to load transfer history.'));
      }
    });
  }

  createDepartment(command: { name: string; maxHeadcount: number }, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.employeeService.createDepartment(command).subscribe({
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

  updateDepartment(id: any, command: { name: string; maxHeadcount: number }, onSuccess: () => void): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.employeeService.updateDepartment(id, command).subscribe({
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

  private refreshEmployees(): void {
    this.employeeService.searchEmployees('').subscribe(emps => {
      this.employeesSubject.next(emps);
    });
  }
}
