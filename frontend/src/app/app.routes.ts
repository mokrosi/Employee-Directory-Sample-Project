import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { EmployeeDirectoryComponent } from './features/employees/containers/employee-directory/employee-directory.component';
import { DepartmentsComponent } from './features/departments/departments.component';
import { HistoryComponent } from './features/history/history.component';
import { authGuard } from './core/guards/auth-guard';
import { employeeResolver } from './features/employees/resolvers/employee-resolver';
import { departmentResolver } from './features/departments/resolvers/department-resolver';
import { historyResolver } from './features/history/resolvers/history-resolver';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'employees',
    component: EmployeeDirectoryComponent,
    canActivate: [authGuard],
    resolve: { initialEmployees: employeeResolver }
  },
  {
    path: 'departments',
    component: DepartmentsComponent,
    canActivate: [authGuard],
    resolve: { initialDepartments: departmentResolver }
  },
  {
    path: 'history',
    component: HistoryComponent,
    canActivate: [authGuard],
    resolve: { initialHistory: historyResolver }
  },
  { path: '', redirectTo: '/employees', pathMatch: 'full' },
  { path: '**', redirectTo: '/employees' }
];
