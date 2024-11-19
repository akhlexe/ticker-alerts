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
import { Subscription } from 'rxjs';
import { ConfirmModalData } from '../confirm-modal/models/confirm-dialog.model';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '../confirm-modal/confirm-modal.component';
import { CurrencyMaskPipe } from '../../pipes/currency-mask.pipe';
import { Router } from '@angular/router';

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


  watchlist: WatchlistDto | null = null;
  private subscription: Subscription | null = null;
  displayedColumns: string[] = ['ticker', 'price', '%', 'actions'];

  constructor(
    private watchlistService: WatchlistService,
    private dialog: MatDialog,
    private router: Router) { }

  ngOnInit(): void {
    this.subscription = this.watchlistService.getWatchlist().subscribe({
      next: (watchlist) => this.watchlist = watchlist,
      error: (err) => console.error('Error observing watchlist state:', err),
    })

    this.watchlistService.loadWatchlist().subscribe({
      error: (err) => console.error('Error loading watchlist:', err),
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
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
}
