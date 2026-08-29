export interface Department {
  id: any;
  name: string;
  maxHeadcount: number;
  currentHeadcount?: number; 
}

export interface Employee {
  id: any;
  employeeCode: string;
  fullName: string;
  email: string;
  departmentId: any;
  departmentName?: string;
  createdByUserId?: any;
  createdAt?: string;
}

export interface EmployeeDepartmentHistory {
  id: any;
  employeeId: any;
  departmentId: any;
  departmentName: string;
  transferredByUserId?: any;
  transferredByUserName?: string;
  transferredAt: string;
}

export interface CreateEmployeeCommand {
  employeeCode: string;
  fullName: string;
  email: string;
  departmentId: any;
}

export interface UpdateEmployeeCommand {
  id: any;
  fullName: string;
  email: string;
}

export interface UpdateDepartmentCommand {
  id: any;
  name: string;
  maxHeadcount: number;
}

export interface TransferEmployeeCommand {
  employeeId: any;
  newDepartmentId?: any;
  targetDepartmentId?: any;
}

export interface HistoryRecord {
  id: any;
  employeeId: any;
  employeeCode: string;
  employeeName: string;
  employeeEmail: string;
  departmentId: any;
  departmentName: string;
  transferredByUserId: any;
  transferredByName: string;
  transferredByEmail: string;
  transferredAt: string;
}

