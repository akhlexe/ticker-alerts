import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { CompanyProfileDto } from '../../shared/services/financial-asset/models/financial-asset.model';
import { FinancialAssetsService } from '../../shared/services/financial-asset/financial-assets.service';

@Component({
  selector: 'app-financial-assets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './financial-assets.component.html',
  styleUrl: './financial-assets.component.css'
})
export class FinancialAssetsComponent implements OnInit {
  companyProfile$: Observable<CompanyProfileDto> | undefined;
  private selectedAssetId = '';

  constructor(private route: ActivatedRoute, private financialAssetsService: FinancialAssetsService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const financialAssetId = params.get('id') || '';
      this.selectedAssetId = financialAssetId;
      this.companyProfile$ = this.financialAssetsService.getFinancialAssetProfile(financialAssetId);
      console.log('Asset ID:', this.selectedAssetId);
    })
  }
}
