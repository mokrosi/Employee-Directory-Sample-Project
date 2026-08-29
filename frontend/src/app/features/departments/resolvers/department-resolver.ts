import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { Department } from '../models/department.model';
import { DepartmentService } from '../services/department.service';

export const departmentResolver: ResolveFn<Department[]> = (): Observable<Department[]> => {
  const departmentService = inject(DepartmentService);
  return departmentService.getDepartments();
};
