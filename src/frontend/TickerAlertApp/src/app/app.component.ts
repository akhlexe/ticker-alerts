import { AuthService } from './core/services/auth.service';
import { SignalRService } from './core/services/signal-r.service';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { NotificationComponent } from './shared/components/notification/notification.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, NotificationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'TickerAlertApp';

  constructor(private authService: AuthService, private signalRService: SignalRService) { }

  ngOnInit(): void {

    debugger
    this.authService.loggedInUsername$.subscribe(username => {
      if (username) {
        this.signalRService.startConnection();
      } else {
        this.signalRService.stopConnection();
      }
    })
  }
}
