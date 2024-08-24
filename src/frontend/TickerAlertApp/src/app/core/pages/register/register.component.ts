import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RegisterRequestDto } from '../../services/models/auth.model';
import { NotificationService } from '../../services/notification/notification.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatCardModule, ReactiveFormsModule, MatInputModule, RouterModule, MatButtonModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onRegister(): void {
    if (this.registerForm.valid) {

      const request: RegisterRequestDto = {
        username: this.registerForm.get('email')?.value,
        password: this.registerForm.get('password')?.value
      }

      this.authService.register(request).subscribe({
        next: (res) => {
          if (res.success) {
            this.route.navigateByUrl('');
            this.notificationService.showSuccess('Registration success!', 'Registration');
          }
          else {
            this.notificationService.showError(res.errorMessage, 'Registration');
          }
        },
        error: (err) => {
          this.notificationService.showError('Error trying to register the user.', 'Registration');
        }
      })

    } else {
      this.notificationService.showError('Invalid form.', 'Registration');
    }
  }

}
