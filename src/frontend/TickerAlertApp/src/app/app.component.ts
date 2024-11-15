import { AuthService } from './core/services/auth.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalRService } from './core/services/signal-r.service';
import { GlobalTickerSearchService } from './shared/components/global-ticker-search/services/global-ticker-search.service';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NotificationComponent } from './shared/components/notification/notification.component';
import { WatchlistComponent } from './shared/components/watchlist/watchlist.component';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent, NotificationComponent, WatchlistComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'TickerAlertApp';
  isLoggedIn!: Observable<string | null>;

  constructor(
    private signalRService: SignalRService,
    private tickerSearchService: GlobalTickerSearchService,
    private authService: AuthService) {

  }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.getLoggedInUsername();
  }

  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }
}