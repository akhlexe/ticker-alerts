import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { WatchlistDto } from './models/watchlist.model';
import { WatchlistHttpService } from './watchlist-http.service';

@Injectable({
  providedIn: 'root'
})
export class WatchlistService {
  private watchlist$ = new BehaviorSubject<WatchlistDto | null>(null);
  private currentWatchlistId: string | null = null;

  constructor(private watchlistHttpService: WatchlistHttpService) { }

  public loadWatchlist(): Observable<WatchlistDto> {
    return this.watchlistHttpService.getWatchlist().pipe(
      tap((watchlist) => {
        this.watchlist$.next(watchlist);
        this.currentWatchlistId = watchlist.id;
      })
    )
  }

  public getWatchlist(): Observable<WatchlistDto | null> {
    return this.watchlist$.asObservable();
  }

  public addWatchlistItem(financialAssetId: string): void {
    if (!this.currentWatchlistId) {
      throw new Error('No watchlist loaded.');
    }

    this.watchlistHttpService
      .addWatchlistItem(this.currentWatchlistId, financialAssetId)
      .subscribe({
        next: (updatedWatchlist) => this.watchlist$.next(updatedWatchlist),
        error: err => console.log("Error on adding an item to the watchlist.")
      });
  }

  public removeItemFromWatchlist(itemId: string): void {
    if (!this.currentWatchlistId) {
      throw new Error('No watchlist loaded');
    }

    this.watchlistHttpService
      .removeWatchlistItem(this.currentWatchlistId, itemId)
      .subscribe({
        next: (updatedWatchlist) => this.watchlist$.next(updatedWatchlist),
        error: err => console.log("Error on deleting an item to the watchlist.")
      });
  }
}
