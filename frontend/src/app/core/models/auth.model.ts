export interface User {
  id: number;
  fullName: string;
  email: string;
}

export interface AuthResponse {
  token: string;
  user?: User;
  message?: string;
}

export interface RegisterCommand {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginQuery {
  email: string;
  password: string;
}
