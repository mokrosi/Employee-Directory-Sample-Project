import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, AsyncPipe } from '@angular/common';
import { EmployeeStateService } from '../../services/employee-state.service';
import { CreateEmployeeCommand, Employee, TransferEmployeeCommand } from '../../models/employee.model';
import { SearchBarComponent } from '../../components/search-bar/search-bar.component';
import { EmployeeListComponent } from '../../components/employee-list/employee-list.component';
import { TransferModalComponent } from '../../components/transfer-modal/transfer-modal.component';
import { EmployeeFormComponent } from '../../components/employee-form/employee-form.component';
import { EmployeeProfileModalComponent } from '../../components/employee-profile-modal/employee-profile-modal.component';

@Component({
  selector: 'app-employee-directory',
  standalone: true,
  imports: [
    CommonModule,
    AsyncPipe,
    SearchBarComponent,
    EmployeeListComponent,
    TransferModalComponent,
    EmployeeFormComponent,
    EmployeeProfileModalComponent
  ],
  templateUrl: './employee-directory.component.html',
  styleUrls: ['./employee-directory.component.css']
})
export class EmployeeDirectoryComponent implements OnInit {
  stateService = inject(EmployeeStateService);

  isAddModalOpen = false;
  viewProfileEmployee: Employee | null = null;
  transferTargetEmployee: Employee | null = null;
  successMessage: string | null = null;

  ngOnInit(): void {
    this.stateService.loadDepartments();
  }

  onSearchChange(term: string): void {
    this.stateService.setSearchTerm(term);
  }

  onOpenAddModal(): void {
    this.isAddModalOpen = true;
  }

  onCloseAddModal(): void {
    this.isAddModalOpen = false;
  }

  onOpenViewModal(employee: Employee): void {
    this.viewProfileEmployee = employee;
  }

  onCloseViewModal(): void {
    this.viewProfileEmployee = null;
  }

  onOpenTransferModal(employee: Employee): void {
    this.transferTargetEmployee = employee;
  }

  onCloseTransferModal(): void {
    this.transferTargetEmployee = null;
  }

  onExecuteTransfer(command: TransferEmployeeCommand): void {
    this.stateService.transferEmployee(command, () => {
      this.onCloseTransferModal();
      this.showSuccess('Employee transferred successfully!');
    });
  }

  onCreateEmployee(command: CreateEmployeeCommand): void {
    this.stateService.createEmployee(command, () => {
      this.onCloseAddModal();
      this.showSuccess('Employee created successfully!');
    });
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => this.successMessage = null, 4000);
  }
}
