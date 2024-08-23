import { MatToolbarModule } from '@angular/material/toolbar';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit, OnDestroy {
  public username: string | null = null;
  private authSubscription: Subscription = new Subscription();

  constructor(private authService: AuthService, private route: Router) { }

  ngOnInit(): void {
    this.authSubscription = this.authService.getLoggedInUsername().subscribe(username => {
      this.username = username;
    });
  }

  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
  }

  public onLogout() {
    this.authService.logout();
    this.route.navigateByUrl("/login");
  }
}
