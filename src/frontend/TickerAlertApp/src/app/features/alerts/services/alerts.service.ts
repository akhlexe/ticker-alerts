import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Alert } from '../models/alert.model';
import { Observable } from 'rxjs';
import { Endpoints } from '../../../constants/api-endpoints.constants';

@Injectable({
  providedIn: 'root',
})
export class AlertsService {
  constructor(private httpClient: HttpClient) {}

  public getAlerts(): Observable<Alert[]> {
    let query = Endpoints.Alerts;
    return this.httpClient.get<Alert[]>(query);
  }
}
