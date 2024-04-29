import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequestDto, RegisterRequestDto } from './models/auth.model';
import { AuthEndpoints } from '../../constants/api-endpoints.constants';
import { StorageKeys } from './models/storage-key.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedInUsername = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
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
          this.storeJwtToken(res.token);
          this.storeUsername(res.username);
          this.loggedInUsername.next(res.username);
        }
      }));
  }

  public login(request: LoginRequestDto) {
    return this.http.post<AuthResponse>(AuthEndpoints.Login, request)
      .pipe(tap(res => {
        if (res.success) {
          this.storeJwtToken(res.token);
          this.storeUsername(res.username);
          this.loggedInUsername.next(res.username);
        }
      }));
  }

  public logout(): void {
    localStorage.removeItem(StorageKeys.JWT_TOKEN);
    localStorage.removeItem(StorageKeys.USERNAME);
    this.loggedInUsername.next(null);
  }

  getLoggedInUsername(): Observable<string | null> {
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

  private storeJwtToken(jwt: string) {
    localStorage.setItem(StorageKeys.JWT_TOKEN, jwt);
  }
  private storeUsername(username: string) {
    localStorage.setItem(StorageKeys.USERNAME, username);
  }
}
