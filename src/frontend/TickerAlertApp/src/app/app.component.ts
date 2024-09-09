import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalRService } from './core/services/signal-r.service';
import { GlobalTickerSearchService } from './shared/components/global-ticker-search/services/global-ticker-search.service';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NotificationComponent } from './shared/components/notification/notification.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, NotificationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'TickerAlertApp';

  constructor(
    private signalRService: SignalRService,
    private tickerSearchService: GlobalTickerSearchService) {

  }
  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }

  ngOnInit(): void {

  }
}