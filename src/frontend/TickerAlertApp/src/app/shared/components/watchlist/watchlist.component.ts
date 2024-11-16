import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { WatchlistService } from '../../services/watchlist/watchlist.service';
import { WatchlistDto, WatchlistItemDto } from '../../services/watchlist/models/watchlist.model';

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
  imports: [CommonModule, FormsModule, MatModules],
  templateUrl: './watchlist.component.html',
  styleUrl: './watchlist.component.css'
})
export class WatchlistComponent {
  watchlist: WatchlistDto | null = null;
  displayedColumns: string[] = ['ticker', 'price', '%'];

  constructor(private watchlistService: WatchlistService) { }

  ngOnInit(): void {
    this.watchlistService.loadWatchlist().subscribe({
      next: ((watchlist) => this.watchlist = watchlist),
      error: (err) => console.error('Error loading watchlist:', err)
    })
  }
}
