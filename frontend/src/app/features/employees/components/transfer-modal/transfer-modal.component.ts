import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Department, Employee, TransferEmployeeCommand } from '../../models/employee.model';

@Component({
  selector: 'app-transfer-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './transfer-modal.component.html',
  styleUrls: ['./transfer-modal.component.css']
})
export class TransferModalComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input() employee!: Employee;
  @Input() departments: Department[] = [];
  @Output() transfer = new EventEmitter<TransferEmployeeCommand>();
  @Output() close = new EventEmitter<void>();

  transferForm!: FormGroup;
  selectedDept: Department | null = null;

  ngOnInit(): void {
    this.transferForm = this.fb.group({
      targetDepartmentId: ['', [Validators.required]]
    });

    this.transferForm.get('targetDepartmentId')?.valueChanges.subscribe(deptId => {
      this.selectedDept = this.departments.find(d => String(d.id) === String(deptId)) || null;
    });
  }

  get isAtCapacity(): boolean {
    if (!this.selectedDept) return false;
    return (this.selectedDept.currentHeadcount ?? 0) >= this.selectedDept.maxHeadcount;
  }

  isCurrentDepartment(dept: Department): boolean {
    if (!this.employee || !this.employee.departmentId || !dept || !dept.id) return false;
    return String(dept.id).toLowerCase() === String(this.employee.departmentId).toLowerCase();
  }

  onSubmit(): void {
    if (this.transferForm.invalid || this.isAtCapacity) return;

    const targetDeptId = this.transferForm.value.targetDepartmentId;
    const command: TransferEmployeeCommand = {
      employeeId: this.employee.id,
      newDepartmentId: targetDeptId,
      targetDepartmentId: targetDeptId
    };
    this.transfer.emit(command);
  }

  onCancel(): void {
    this.close.emit();
  }
}
