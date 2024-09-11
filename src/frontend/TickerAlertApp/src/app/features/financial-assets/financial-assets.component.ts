import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { FinancialAssetsService } from '../../shared/services/financial-asset/financial-assets.service';
import { CompanyProfileDto } from '../../shared/services/financial-asset/models/financial-asset.model';
import { CreateAlertModalComponent } from '../alerts/components/create-alert-modal/create-alert-modal.component';

const MatModules = [
  MatProgressSpinnerModule,
  MatIcon,
  MatButtonModule,
  MatDialogModule
]

@Component({
  selector: 'app-financial-assets',
  standalone: true,
  imports: [CommonModule, MatModules],
  templateUrl: './financial-assets.component.html',
  styleUrl: './financial-assets.component.css'
})
export class FinancialAssetsComponent implements OnInit {
  companyProfile$: Observable<CompanyProfileDto> | undefined;

  constructor(
    private route: ActivatedRoute,
    private financialAssetsService: FinancialAssetsService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const financialAssetId = params.get('id') || '';
      this.companyProfile$ = this.financialAssetsService.getFinancialAssetProfile(financialAssetId);
    })
  }

  public onCreateAlert() {
    this.dialog.open(CreateAlertModalComponent);
  }
}
