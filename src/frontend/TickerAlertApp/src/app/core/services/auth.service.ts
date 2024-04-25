import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequestDto, LoginResponseDto, RegisterRequestDto, RegisterResponseDto } from './models/auth.model';
import { AuthEndpoints } from '../../constants/api-endpoints.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly JWT_TOKEN = 'JWT_TOKEN';
  constructor(private http: HttpClient) { }

  public register(request: RegisterRequestDto) {
    return this.http.post<RegisterResponseDto>(AuthEndpoints.Register, request)
      .pipe(tap(res => this.storeJwtToken(res.token)));
  }

  public login(request: LoginRequestDto) {
    return this.http.post<LoginResponseDto>(AuthEndpoints.Login, request)
      .pipe(tap(res => this.storeJwtToken(res.token)));
  }

  public logout(): void {
    localStorage.removeItem(this.JWT_TOKEN);
  }

  public isAuth(): boolean {
    return !!localStorage.getItem(this.JWT_TOKEN);
  }

  public getToken(): string | null {
    return localStorage.getItem('token');
  }

  private storeJwtToken(jwt: string) {
    localStorage.setItem(this.JWT_TOKEN, jwt);
  }
}
