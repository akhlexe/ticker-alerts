import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WatchlistEndpoints } from '../../../constants/api-endpoints.constants';
import { WatchlistDto } from './models/watchlist.model';
import { AddItemRequest, RemoveItemRequest } from './models/watchlist-http.model';

@Injectable({
  providedIn: 'root'
})
export class WatchlistHttpService {

  constructor(private httpClient: HttpClient) { }

  public getWatchlist(): Observable<WatchlistDto> {
    return this.httpClient.get<WatchlistDto>(WatchlistEndpoints.GetWatchlist);
  }

  public addWatchlistItem(watchlistId: string, financialAssetId: string): Observable<WatchlistDto> {
    var body: AddItemRequest = {
      watchlistId: watchlistId,
      financialAssetId: financialAssetId
    }

    return this.httpClient.post<WatchlistDto>(WatchlistEndpoints.AddWatchlistItem, body);
  }

  public removeWatchlistItem(watchlistId: string, itemId: string): Observable<WatchlistDto> {

    var body: RemoveItemRequest = {
      watchlistId: watchlistId,
      itemId: itemId
    }

    return this.httpClient.delete<WatchlistDto>(WatchlistEndpoints.RemoveWatchlistItem, {
      body: body
    });
  }
}
