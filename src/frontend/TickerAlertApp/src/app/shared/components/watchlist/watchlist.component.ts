import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { WatchlistService } from '../../services/watchlist/watchlist.service';
import { WatchlistDto, WatchlistItemDto } from '../../services/watchlist/models/watchlist.model';
import { BehaviorSubject, Observable, Subject, Subscription, takeUntil } from 'rxjs';
import { ConfirmModalData } from '../confirm-modal/models/confirm-dialog.model';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '../confirm-modal/confirm-modal.component';
import { CurrencyMaskPipe } from '../../pipes/currency-mask.pipe';
import { Router } from '@angular/router';
import { SignalRService } from '../../../core/services/signal-r.service';
import { AssetPriceUpdateDto } from '../../../core/services/models/signalr.model';

const MatModules = [
  MatCardModule,
  MatFormFieldModule,
  MatInputModule,
  MatButtonModule,
  MatIconModule,
  MatTableModule
]

export interface TrackElement {
  ticker: string;
  price: number;
  ratio: string;
}

@Component({
  selector: 'app-watchlist',
  standalone: true,
  imports: [CommonModule, FormsModule, MatModules, CurrencyMaskPipe],
  templateUrl: './watchlist.component.html',
  styleUrl: './watchlist.component.css'
})
export class WatchlistComponent implements OnInit, OnDestroy {

  public displayedColumns: string[] = ['ticker', 'price', 'link', 'actions'];
  public watchlist$ = this.watchlistService.getWatchlist();

  private destroy$ = new Subject<void>();

  constructor(
    private watchlistService: WatchlistService,
    private dialog: MatDialog,
    private router: Router) { }

  public ngOnInit(): void {
    this.watchlistService.loadWatchlist().pipe(takeUntil(this.destroy$)).subscribe({
      error: (err) => console.error('Error loading watchlist:', err),
    });
  }

  public removeItem(event: MouseEvent, itemId: string) {
    // Requerido para evitar redirección.
    event.stopPropagation();

    const dialogData: ConfirmModalData = {
      title: 'Watchlist',
      message: 'Do you want to remove this item?',
      confirmText: 'Yes',
      cancelText: 'No',
    };
    const dialogRef = this.dialog.open(ConfirmModalComponent, {
      width: '300px',
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.watchlistService.removeItemFromWatchlist(itemId);
      }
    });
  }

  public onRowClick(row: any) {
    this.router.navigate(['financial-assets', row.financialAssetId]);
  }

  public openLink(url: any) {
    debugger
    window.open(url, '_blank');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
