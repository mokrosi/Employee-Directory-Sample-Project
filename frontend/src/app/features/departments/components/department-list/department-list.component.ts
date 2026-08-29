import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Department } from '../../models/department.model';
import { DepartmentCardComponent } from '../department-card/department-card.component';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [CommonModule, DepartmentCardComponent],
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.css']
})
export class DepartmentListComponent {
  @Input() departments: Department[] = [];
  @Output() editDepartment = new EventEmitter<Department>();

  onEdit(dept: Department): void {
    this.editDepartment.emit(dept);
  }
}
