import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WatchlistEndpoints } from '../../../constants/api-endpoints.constants';
import { WatchlistDto } from './models/watchlist.model';

@Injectable({
  providedIn: 'root'
})
export class WatchlistHttpService {

  constructor(private httpClient: HttpClient) { }

  public getWatchlist(): Observable<WatchlistDto> {
    return this.httpClient.get<WatchlistDto>(WatchlistEndpoints.GetWatchlist);
  }

  public addWatchlistItem(watchlistId: string, financialAssetId: string): Observable<WatchlistDto> {
    let params = new HttpParams();
    params = params.append('watchlistId', watchlistId);
    params = params.append('financialAssetId', financialAssetId);

    return this.httpClient.post<WatchlistDto>(WatchlistEndpoints.AddWatchlistItem, { params });
  }

  public removeWatchlistItem(watchlistId: string, itemId: string): Observable<WatchlistDto> {
    let params = new HttpParams();
    params = params.append('watchlistId', watchlistId);
    params = params.append('itemId', itemId);

    return this.httpClient.delete<WatchlistDto>(WatchlistEndpoints.RemoveWatchlistItem, { params });
  }
}
