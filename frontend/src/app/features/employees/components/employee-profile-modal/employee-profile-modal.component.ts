import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Employee } from '../../models/employee.model';

@Component({
  selector: 'app-employee-profile-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './employee-profile-modal.component.html',
  styleUrls: ['./employee-profile-modal.component.css']
})
export class EmployeeProfileModalComponent {
  @Input() employee!: Employee;
  @Output() close = new EventEmitter<void>();

  onClose(): void {
    this.close.emit();
  }
}
