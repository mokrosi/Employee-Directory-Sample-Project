import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateEmployeeCommand, Department } from '../../models/employee.model';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input() departments: Department[] = [];
  @Output() createEmployee = new EventEmitter<CreateEmployeeCommand>();
  @Output() close = new EventEmitter<void>();

  employeeForm!: FormGroup;

  ngOnInit(): void {
    this.employeeForm = this.fb.group({
      employeeCode: ['', [Validators.required]],
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      departmentId: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formVal = this.employeeForm.value;
    const command: CreateEmployeeCommand = {
      employeeCode: formVal.employeeCode?.trim(),
      fullName: formVal.fullName?.trim(),
      email: formVal.email?.trim(),
      departmentId: formVal.departmentId
    };

    this.createEmployee.emit(command);
    this.employeeForm.reset({ employeeCode: '', fullName: '', email: '', departmentId: '' });
  }

  onCancel(): void {
    this.employeeForm.reset({ employeeCode: '', fullName: '', email: '', departmentId: '' });
    this.close.emit();
  }
}
