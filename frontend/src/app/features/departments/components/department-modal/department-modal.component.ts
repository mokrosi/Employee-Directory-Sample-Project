import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-department-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './department-modal.component.html',
  styleUrls: ['./department-modal.component.css']
})
export class DepartmentModalComponent implements OnInit, OnChanges {
  private fb = inject(FormBuilder);

  @Input() isEditing: boolean = false;
  @Input() department: Department | null = null;
  @Input() isLoading: boolean = false;
  @Output() save = new EventEmitter<{ name: string; maxHeadcount: number }>();
  @Output() close = new EventEmitter<void>();

  departmentForm!: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['department'] && this.departmentForm) {
      this.initForm();
    }
  }

  private initForm(): void {
    this.departmentForm = this.fb.group({
      name: [this.department?.name || '', [Validators.required, Validators.maxLength(100)]],
      maxHeadcount: [this.department?.maxHeadcount || 5, [Validators.required, Validators.min(1)]]
    });
  }

  onSubmit(): void {
    if (this.departmentForm.invalid) {
      this.departmentForm.markAllAsTouched();
      return;
    }

    const payload = {
      name: this.departmentForm.value.name.trim(),
      maxHeadcount: Number(this.departmentForm.value.maxHeadcount)
    };

    this.save.emit(payload);
  }

  onCancel(): void {
    this.close.emit();
  }
}
