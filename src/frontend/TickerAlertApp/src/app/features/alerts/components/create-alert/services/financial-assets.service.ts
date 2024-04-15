import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FinancialAssetDto } from '../models/financial-asset.model';
import { Endpoints } from '../../../../../constants/api-endpoints.constants';

@Injectable({
  providedIn: 'root',
})
export class FinancialAssetsService {
  constructor(private httpClient: HttpClient) {}

  public getFinancialAssetsByCriteria(
    criteria: string
  ): Observable<FinancialAssetDto[]> {
    let params = new HttpParams();
    params = params.append('criteria', criteria);

    return this.httpClient.get<FinancialAssetDto[]>(Endpoints.FinancialAssets, {
      params,
    });
  }
}
