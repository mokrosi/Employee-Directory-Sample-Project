export interface Department {
  id: any;
  name: string;
  maxHeadcount: number;
  currentHeadcount?: number;
}

export interface CreateDepartmentCommand {
  name: string;
  maxHeadcount: number;
}

export interface UpdateDepartmentCommand {
  id?: any;
  name: string;
  maxHeadcount: number;
}
