import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, Subscription, tap } from 'rxjs';
import { WatchlistDto, WatchlistItemDto } from './models/watchlist.model';
import { WatchlistHttpService } from './watchlist-http.service';
import { SignalRService } from '../../../core/services/signal-r.service';
import { AssetPriceUpdateDto } from '../../../core/services/models/signalr.model';

@Injectable({
  providedIn: 'root'
})
export class WatchlistService implements OnDestroy {
  private watchlistSubject = new BehaviorSubject<WatchlistDto | null>(null);
  private currentWatchlistId: string | null = null;
  private priceUpdateSubscription: Subscription | null = null;

  constructor(
    private watchlistHttpService: WatchlistHttpService,
    private signalRService: SignalRService) {
    this.listenForPriceUpdates();
  }

  public loadWatchlist(): Observable<WatchlistDto> {
    return this.watchlistHttpService.getWatchlist().pipe(
      tap((watchlist) => {
        this.watchlistSubject.next(watchlist);
        this.currentWatchlistId = watchlist.id;
      })
    )
  }

  public getWatchlist(): Observable<WatchlistDto | null> {
    return this.watchlistSubject.asObservable();
  }

  public addWatchlistItem(financialAssetId: string): void {
    if (!this.currentWatchlistId) {
      throw new Error('No watchlist loaded.');
    }

    this.watchlistHttpService
      .addWatchlistItem(this.currentWatchlistId, financialAssetId)
      .subscribe({
        next: (updatedWatchlist) => this.watchlistSubject.next(updatedWatchlist),
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
        next: (updatedWatchlist) => this.watchlistSubject.next(updatedWatchlist),
        error: err => console.log("Error on deleting an item to the watchlist.")
      });
  }

  public listenForPriceUpdates(): void {
    this.priceUpdateSubscription = this.signalRService.assetPriceUpdate$.subscribe({
      next: (priceUpdate) => this.updatePriceInWatchlist(priceUpdate),
      error: (err) => console.log('Error receiving price updates: ', err)
    });
  }

  private updatePriceInWatchlist(priceUpdate: AssetPriceUpdateDto | null): void {
    if (!priceUpdate) return;

    const currentWatchlist = this.watchlistSubject.value;
    if (!currentWatchlist) return;

    const updatedItems = currentWatchlist.items.map(item =>
      item.financialAssetId === priceUpdate.financialAssetId
        ? { ...item, price: priceUpdate.newPrice }
        : item
    );

    const updatedWatchlist = { ...currentWatchlist, items: updatedItems };

    this.watchlistSubject.next(updatedWatchlist);
  }

  ngOnDestroy(): void {
    this.priceUpdateSubscription?.unsubscribe();
  }
}
