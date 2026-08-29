import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { EmployeeService } from '../services/employee.service';
import { EmployeeStateService } from '../services/employee-state.service';
import { Employee } from '../models/employee.model';

export const employeeResolver: ResolveFn<Employee[]> = (): Observable<Employee[]> => {
  const employeeService = inject(EmployeeService);
  const employeeState = inject(EmployeeStateService);

  return employeeService.searchEmployees('').pipe(
    tap(employees => employeeState.setInitialEmployees(employees))
  );
};
