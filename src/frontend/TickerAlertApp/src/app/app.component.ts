import { GlobalKeydownService } from './core/services/global-keydown.service';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalRService } from './core/services/signal-r.service';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NotificationComponent } from './shared/components/notification/notification.component';
import { MatDialog } from '@angular/material/dialog';
import { GlobalSearchComponent } from './shared/components/search-ticker/components/global-search/global-search.component';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, NotificationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'TickerAlertApp';
  private modalRef: any;

  constructor(
    private signalRService: SignalRService,
    private dialog: MatDialog,
    private keydownService: GlobalKeydownService) {

  }
  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }

  ngOnInit(): void {

    this.keydownService.getKeydownObservable().subscribe(event => {

      const activeElement = document.activeElement;
      const isInputFocused = activeElement instanceof HTMLInputElement || activeElement instanceof HTMLTextAreaElement;

      if (!isInputFocused && !this.modalRef) {
        this.openModal(event.key);
      }
    })
  }

  private openModal(initialKey: string) {
    this.modalRef = this.dialog.open(GlobalSearchComponent, {
      data: { initialInput: initialKey },
    });

    this.modalRef.afterClosed().subscribe(() => {
      this.modalRef = null;
    });
  }
}