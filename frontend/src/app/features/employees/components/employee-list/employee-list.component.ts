import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Employee } from '../../models/employee.model';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent {
  @Input() employees: Employee[] = [];
  @Output() employeeSelected = new EventEmitter<Employee>();
  @Output() openViewModal = new EventEmitter<Employee>();
  @Output() openTransferModal = new EventEmitter<Employee>();

  onSelect(employee: Employee): void {
    this.employeeSelected.emit(employee);
  }

  onView(employee: Employee, event: MouseEvent): void {
    event.stopPropagation();
    this.openViewModal.emit(employee);
  }

  onTransfer(employee: Employee, event: MouseEvent): void {
    event.stopPropagation();
    this.openTransferModal.emit(employee);
  }
}
