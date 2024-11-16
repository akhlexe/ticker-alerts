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

  public addWatchlistItem(financialAssetId: string): Observable<WatchlistDto> {
    if (!this.currentWatchlistId) {
      throw new Error('No watchlist loaded.');
    }

    return this.watchlistHttpService
      .addWatchlistItem(this.currentWatchlistId, financialAssetId)
      .pipe(
        tap((updatedWatchlist) => this.watchlist$.next(updatedWatchlist))
      )
  }

  public removeItemFromWatchlist(itemId: string): Observable<WatchlistDto> {
    if (!this.currentWatchlistId) {
      throw new Error('No watchlist loaded');
    }

    return this.watchlistHttpService
      .removeWatchlistItem(this.currentWatchlistId, itemId)
      .pipe(
        tap((updatedWatchlist) => this.watchlist$.next(updatedWatchlist))
      );
  }
}
