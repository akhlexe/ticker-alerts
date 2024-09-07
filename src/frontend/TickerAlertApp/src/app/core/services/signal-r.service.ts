import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StorageKeys } from './models/storage-key.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: HubConnection;
  // TODO: Remover notification subject que es usado para el evento de ReceiveMessage
  // que debería ser renombrado a un servicio para mostrar alertas que se reciba.
  // Se debería llamar AlertReceived, y tener su propio subject en ese servicio que se dispare cuando 
  // reciba la notificacion. Esta clase no debería tener ningún subject.
  private notificationSubject = new BehaviorSubject<string | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  constructor(private authService: AuthService) {
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

    this.hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing signalr connection to the server.'));
  }

  public stopConnection(): void {
    this.hubConnection.stop()
      .then(() => console.log('Hub connection stopped'))
      .catch(err => console.log('Error while stopping connection: ' + err));
  }

  public on(event: string, callback: (...args: any[]) => void): void {
    this.hubConnection.on(event, callback);
  }

  public invoke(methodName: string, ...args: any[]): Promise<void> {
    return this.hubConnection.invoke(methodName, ...args);
  }

  public isConnected(): boolean {
    return this.hubConnection && this.hubConnection.state === HubConnectionState.Connected;
  }
}
