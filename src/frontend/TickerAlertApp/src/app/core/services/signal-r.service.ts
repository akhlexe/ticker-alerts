import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StorageKeys } from './models/storage-key.model';
import { AuthService } from './auth.service';
import { AssetPriceUpdateDto } from './models/signalr.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: HubConnection;
  private notificationSubject = new BehaviorSubject<string | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  private assetPriceUpdateSubject = new BehaviorSubject<AssetPriceUpdateDto | null>(null);
  public assetPriceUpdate$ = this.assetPriceUpdateSubject.asObservable();

  constructor(private authService: AuthService) {
    this.connectToSignalRWhenUserIsLoggedIn();
  }

  private connectToSignalRWhenUserIsLoggedIn() {
    this.authService.getLoggedInUsername().subscribe(username => {
      if (username) {
        this.startConnection();
      } else {
        if (this.hubConnection) {
          this.stopConnection();
        }
      }
    });
  }

  public startConnection(): void {
    const token = localStorage.getItem(StorageKeys.JWT_TOKEN);

    if (!token) {
      console.error('No token found in session storage');
      return;
    }

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apiProxy + '/tickerbloomhub', {
        accessTokenFactory: () => token,
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.hubConnection.on('ReceiveMessage', (message) => {
      this.notificationSubject.next(message);
    })

    this.hubConnection.on('ReceiveAssetPriceUpdate', (assetPriceUpdate) => {
      this.assetPriceUpdateSubject.next(assetPriceUpdate);
    })

    this.hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing signalr connection to the server.'));
  }

  public stopConnection(): void {
    this.hubConnection.stop()
      .then(() => console.log('Hub connection stopped'))
      .catch(err => console.log('Error while stopping connection: ' + err));
  }
}
