import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { RegisterResponse } from './model/register-response';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginResponse } from './model/login-response';
import { environment } from '../../environments/environment';
import { LoginRequest } from './model/login-request';
import { RegisterRequest } from './model/register-request';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = `${environment.apiUrl}/auth`;

  private loggedIn$ = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
  isLoggedIn$ = this.loggedIn$.asObservable();

  register(data: RegisterRequest): Observable<RegisterResponse> {
    return this.http
      .post<RegisterResponse>(`${this.apiUrl}/register`, data)
      .pipe(tap(() => this.router.navigate(['/login'])));
  }

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data).pipe(
      tap((res) => {
        localStorage.setItem('token', res.token);
        this.loggedIn$.next(true);
        this.router.navigate(['/events']);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.loggedIn$.next(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.uid || payload.userId || payload.sub || null;
    } catch {
      return null;
    }
  }
}
