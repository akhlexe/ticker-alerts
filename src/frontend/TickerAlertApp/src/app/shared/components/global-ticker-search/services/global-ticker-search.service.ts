import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { GlobalKeydownService } from './../../../../core/services/global-keydown.service';
import { Injectable } from '@angular/core';
import { GlobalSearchComponent } from '../global-search.component';

@Injectable({
  providedIn: 'root'
})
export class GlobalTickerSearchService {

  private modalRef: any;

  constructor(private keydownService: GlobalKeydownService, private dialog: MatDialog) {
    this.listenToKeydown();
  }

  private listenToKeydown(): void {
    this.keydownService.getKeydownObservable()
      .pipe(filter(event => this.shouldOpenModal(event)))
      .subscribe(event => this.openModal(event.key));
  }

  private shouldOpenModal(event: KeyboardEvent): boolean {
    const isLetter = /^[a-zA-Z]$/.test(event.key);
    const activeElement = document.activeElement;
    const isInputFocused = activeElement instanceof HTMLInputElement || activeElement instanceof HTMLTextAreaElement;
    return isLetter && !isInputFocused && !this.modalRef;
  }

  private openModal(initialKey: string): void {
    this.modalRef = this.dialog.open(GlobalSearchComponent, {
      data: { initialInput: initialKey },
    });

    this.modalRef.afterClosed().subscribe(() => {
      this.modalRef = null;
    });
  }
}
