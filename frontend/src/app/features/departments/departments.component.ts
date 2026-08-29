import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, AsyncPipe } from '@angular/common';
import { DepartmentStateService } from './services/department-state.service';
import { Department } from './models/department.model';
import { DepartmentListComponent } from './components/department-list/department-list.component';
import { DepartmentModalComponent } from './components/department-modal/department-modal.component';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [
    CommonModule,
    AsyncPipe,
    DepartmentListComponent,
    DepartmentModalComponent
  ],
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  public stateService = inject(DepartmentStateService);

  isModalOpen = false;
  isEditing = false;
  selectedDepartment: Department | null = null;
  successMessage: string | null = null;

  ngOnInit(): void {
    this.stateService.loadDepartments();
  }

  openCreateModal(): void {
    this.isEditing = false;
    this.selectedDepartment = null;
    this.isModalOpen = true;
  }

  openEditModal(dept: Department): void {
    this.isEditing = true;
    this.selectedDepartment = dept;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedDepartment = null;
  }

  onSaveDepartment(payload: { name: string; maxHeadcount: number }): void {
    if (this.isEditing && this.selectedDepartment) {
      this.stateService.updateDepartment(this.selectedDepartment.id, payload, () => {
        this.closeModal();
        this.showSuccess(`Department "${payload.name}" updated successfully!`);
      });
    } else {
      this.stateService.createDepartment(payload, () => {
        this.closeModal();
        this.showSuccess(`Department "${payload.name}" created successfully!`);
      });
    }
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => this.successMessage = null, 4000);
  }
}
