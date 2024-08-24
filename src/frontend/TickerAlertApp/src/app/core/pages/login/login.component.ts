import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequestDto } from '../../services/models/auth.model';
import { NotificationService } from '../../services/notification/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatInputModule, ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const request: LoginRequestDto = {
        username: this.loginForm.get('email')?.value,
        password: this.loginForm.get('password')?.value
      }

      this.authService.login(request).subscribe({
        next: (res) => {
          if (res.success) {
            this.route.navigateByUrl('');
            this.notificationService.showSuccess('Login success!', 'Authentication')
          }
          else {
            this.notificationService.showError(res.errorMessage, 'Authentication')
          }
        },
        error: (err) => {
          this.notificationService.showError('Error trying to login the user.', 'Authentication')
        }
      })
    }
  }
}
