import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';

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
  watchlist: TrackElement[] = [];
  newItem: string = '';
  displayedColumns: string[] = ['ticker', 'price', 'ratio'];

  ngOnInit(): void {
    // Initialize with some items or fetch from a service
    this.watchlist = [
      {
        ticker: "MSFT",
        price: 420,
        ratio: "3:1"
      },
      {
        ticker: "GOOGL",
        price: 165.57,
        ratio: "8:1"
      }
    ];
  }

  addItem(): void {
    if (this.newItem.trim()) {
      // this.watchlist.push(this.newItem.trim());
      this.newItem = '';
    }
  }

  removeItem(item: string): void {
    // this.watchlist = this.watchlist.filter(i => i !== item);
  }
}
