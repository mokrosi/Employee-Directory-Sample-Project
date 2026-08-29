import { Component, Input } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Employee, EmployeeDepartmentHistory } from '../../models/employee.model';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css']
})
export class EmployeeDetailComponent {
  @Input() employee: Employee | null = null;
  @Input() history: EmployeeDepartmentHistory[] = [];
}
