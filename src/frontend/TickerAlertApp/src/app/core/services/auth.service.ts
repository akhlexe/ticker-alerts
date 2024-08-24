import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequestDto, RegisterRequestDto } from './models/auth.model';
import { AuthEndpoints } from '../../constants/api-endpoints.constants';
import { StorageKeys } from './models/storage-key.model';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedInUsername = new BehaviorSubject<string | null>(null);

  constructor(
    private http: HttpClient,
    private route: Router) {
    this.checkInitialAuthState();
  }

  private checkInitialAuthState() {
    const username = localStorage.getItem(StorageKeys.USERNAME);
    this.loggedInUsername.next(username);
  }

  public register(request: RegisterRequestDto) {
    return this.http.post<AuthResponse>(AuthEndpoints.Register, request)
      .pipe(tap(res => {
        if (res.success) {
          this.saveCredentials(res.token, res.username);
          this.loggedInUsername.next(res.username);
        }
      }));
  }

  public login(request: LoginRequestDto) {
    return this.http.post<AuthResponse>(AuthEndpoints.Login, request)
      .pipe(tap(res => {
        if (res.success) {
          this.saveCredentials(res.token, res.username);
          this.loggedInUsername.next(res.username);
        }
      }));
  }

  public logout(): void {
    this.cleanCredentials();
    this.loggedInUsername.next(null);
    this.route.navigateByUrl("/login");
  }

  public isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const expiry = (JSON.parse(atob(token.split('.')[1]))).exp;
    return expiry * 1000 <= Date.now();
  }

  public getLoggedInUsername(): Observable<string | null> {
    return this.loggedInUsername.asObservable();
  }

  public isAuth(): boolean {
    return !!localStorage.getItem(StorageKeys.JWT_TOKEN);
  }

  public getToken(): string | null {
    return localStorage.getItem(StorageKeys.JWT_TOKEN);
  }

  public getLoggedInUsename(): string | null {
    return localStorage.getItem(StorageKeys.USERNAME);
  }

  private saveCredentials(token: string, username: string) {
    localStorage.setItem(StorageKeys.JWT_TOKEN, token);
    localStorage.setItem(StorageKeys.USERNAME, username);
  }

  private cleanCredentials() {
    localStorage.removeItem(StorageKeys.JWT_TOKEN);
    localStorage.removeItem(StorageKeys.USERNAME);
  }
}
