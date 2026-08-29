import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthResponse, LoginQuery, RegisterCommand, User } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'https://localhost:7214/api/auth';

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  currentUser$ = this.currentUserSubject.asObservable();
  isLoading$ = this.loadingSubject.asObservable();
  error$ = this.errorSubject.asObservable();

  constructor() {
    this.restoreSession();
  }

  register(command: RegisterCommand): Observable<any> {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return this.http.post(`${this.apiUrl}/register`, command).pipe(
      tap(() => this.loadingSubject.next(false)),
      catchError((error: HttpErrorResponse) => {
        this.loadingSubject.next(false);
        const errorMsg = error.error?.message || error.error || 'Registration failed.';
        this.errorSubject.next(errorMsg);
        return throwError(() => new Error(errorMsg));
      })
    );
  }

  login(query: LoginQuery): Observable<AuthResponse> {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, query).pipe(
      tap(response => {
        this.loadingSubject.next(false);
        if (response && response.token) {
          localStorage.setItem('jwt_token', response.token);
          const user = response.user || this.getUserFromToken(response.token);
          if (user) {
            localStorage.setItem('auth_user', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
          this.router.navigate(['/employees']);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.loadingSubject.next(false);
        const errorMsg = error.error?.message || error.error?.detail || error.error || 'Invalid credentials.';
        this.errorSubject.next(errorMsg);
        return throwError(() => new Error(errorMsg));
      })
    );
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('auth_user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private getUserFromToken(token: string): User | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.sub || payload.nameid,
        fullName: payload.FullName || payload.name || payload.email || 'User',
        email: payload.email || ''
      };
    } catch {
      return null;
    }
  }

  private restoreSession(): void {
    const token = this.getToken();
    if (token) {
      const savedUser = localStorage.getItem('auth_user');
      if (savedUser) {
        try {
          this.currentUserSubject.next(JSON.parse(savedUser));
        } catch {
          const user = this.getUserFromToken(token);
          if (user) this.currentUserSubject.next(user);
        }
      } else {
        const user = this.getUserFromToken(token);
        if (user) {
          this.currentUserSubject.next(user);
        }
      }
    }
  }
}
