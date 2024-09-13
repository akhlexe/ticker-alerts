import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompanyProfileDto, FinancialAssetDto } from './models/financial-asset.model';
import { Endpoints } from '../../../constants/api-endpoints.constants';
import { Result } from '../../models/result.models';

@Injectable({
  providedIn: 'root',
})
export class FinancialAssetsService {
  constructor(private httpClient: HttpClient) { }

  public getFinancialAssetsByCriteria(criteria: string): Observable<FinancialAssetDto[]> {
    let params = new HttpParams();
    params = params.append('criteria', criteria);

    return this.httpClient.get<FinancialAssetDto[]>(Endpoints.FinancialAssets, { params });
  }

  public getFinancialAssetProfile(financialAssetId: string): Observable<CompanyProfileDto> {
    let params = new HttpParams();
    params = params.append('financialAssetId', financialAssetId);

    return this.httpClient.get<CompanyProfileDto>(Endpoints.FinancialAssetProfile, { params });
  }

  public getFinancialAssetById(id: string): Observable<Result<FinancialAssetDto>> {
    const query = Endpoints.FinancialAssetById.replace("{id}", id);
    return this.httpClient.get<Result<FinancialAssetDto>>(query);
  }
}
