import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-department-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './department-card.component.html',
  styleUrls: ['./department-card.component.css']
})
export class DepartmentCardComponent {
  @Input() department!: Department;
  @Output() edit = new EventEmitter<Department>();

  onEdit(): void {
    this.edit.emit(this.department);
  }
}
