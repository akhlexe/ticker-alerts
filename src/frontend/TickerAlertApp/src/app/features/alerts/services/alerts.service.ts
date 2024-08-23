import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Alert } from '../models/alert.model';
import { Observable } from 'rxjs';
import { Endpoints } from '../../../constants/api-endpoints.constants';
import { CreateAlertRequest } from '../components/create-alert/models/create-alert.models';
import { Result } from '../../../shared/models/result.models';
import { CancelAlertRequest, ConfirmReceptionRequest } from './models/requests.model';

@Injectable({
  providedIn: 'root',
})
export class AlertsService {

  constructor(private httpClient: HttpClient) { }

  public getAlerts(): Observable<Alert[]> {
    let query = Endpoints.Alerts;
    return this.httpClient.get<Alert[]>(query);
  }

  public createAlert(request: CreateAlertRequest): Observable<Result> {
    return this.httpClient.post<Result>(Endpoints.CreateAlert, request);
  }

  public cancelAlert(request: CancelAlertRequest): Observable<Result> {
    return this.httpClient.post<Result>(Endpoints.CancelAlert, request);
  }

  public confirmReception(request: ConfirmReceptionRequest): Observable<Result> {
    return this.httpClient.post<Result>(Endpoints.ConfirmReception, request);
  }
}
