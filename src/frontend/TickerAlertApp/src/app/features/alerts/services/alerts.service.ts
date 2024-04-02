import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Alert } from '../models/alert.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AlertsService {
  constructor(private httpClient: HttpClient) {}

  public getAlerts(): Observable<Alert[]> {
    let query = 'https://localhost:7279/api/Alerts';
    return this.httpClient.get<Alert[]>(query);
  }
}
