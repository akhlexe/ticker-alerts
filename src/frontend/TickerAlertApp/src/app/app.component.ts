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
  private modalSubject = new Subject<string>();
  private modalRef: any;

  constructor(private signalRService: SignalRService, private dialog: MatDialog) {
    // this.listenToKeydown();

  }
  ngOnDestroy(): void {
    this.signalRService.stopConnection();

  }

  ngOnInit(): void {
    this.modalSubject.subscribe((key: string) => {
      if (this.modalRef) {
        this.modalRef.componentInstance.inputValue += key;
      } else {
        this.openModal(key);
      }
    });
  }

  @HostListener('window:keydown', ['$event'])
  public handleKeydown(event: KeyboardEvent) {
    const isLetter = /^[a-zA-Z]$/.test(event.key);
    if (isLetter) {
      console.log(event.key);
      this.modalSubject.next(event.key);
    }
  }

  openModal(initialKey: string) {
    this.modalRef = this.dialog.open(GlobalSearchComponent, {
      data: { initialInput: initialKey },
    });

    this.modalRef.afterClosed().subscribe(() => {
      this.modalRef = null;
    });
  }
}