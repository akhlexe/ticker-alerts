import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: HubConnection;
  private notificationSubject = new BehaviorSubject<string | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  constructor() { }

  public startConnection(): void {

    debugger
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apiBaseUrl + '/alertTriggeredHub')
      .build();

    this.hubConnection.on('MessageReceived', (message) => {
      console.log(message);
    })

    this.hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing signalr connection to the server.'));
  }

  public stopConnection(): void {

    debugger
    this.hubConnection.stop()
      .then(() => console.log('Hub connection stopped'))
      .catch(err => console.log('Error while stopping connection: ' + err));
  }
}
