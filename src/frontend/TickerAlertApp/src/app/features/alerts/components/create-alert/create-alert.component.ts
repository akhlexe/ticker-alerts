import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  AfterViewInit,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CreateAlertRequest } from './models/create-alert.models';
import { AlertsService } from '../../services/alerts.service';
import { FinancialAssetsService } from './services/financial-assets.service';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Observable, debounceTime, filter, switchMap } from 'rxjs';
import { FinancialAssetDto } from './models/financial-asset.model';

@Component({
  selector: 'app-create-alert',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    CommonModule,
    MatButtonModule,
    MatAutocompleteModule,
  ],
  templateUrl: './create-alert.component.html',
  styleUrl: './create-alert.component.css',
  providers: [AlertsService, FinancialAssetsService],
})
export class CreateAlertComponent implements OnInit, AfterViewInit {
  createAlertForm!: FormGroup;
  assetSearchList: Observable<FinancialAssetDto[]> | undefined;

  @ViewChild('tickerInput') tickerInput!: ElementRef;

  constructor(
    private formBuilder: FormBuilder,
    private alertsService: AlertsService,
    private assetsService: FinancialAssetsService
  ) {}
  ngAfterViewInit(): void {
    setTimeout(() => this.tickerInput.nativeElement.focus());
  }

  ngOnInit(): void {
    this.createAlertForm = this.formBuilder.group({
      ticker: [null, []],
      targetPrice: ['', []],
    });

    this.assetSearchList = this.createAlertForm
      .get('ticker')
      ?.valueChanges.pipe(
        debounceTime(300),
        filter((value) => value && value.length > 2),
        switchMap((value) =>
          this.assetsService.getFinancialAssetsByCriteria(value)
        )
      );
  }

  public isCreateAlertFormValid() {
    const ticker = this.createAlertForm.get('ticker')?.value;
    const targetPrice = this.createAlertForm.get('targetPrice')?.value;

    return ticker && targetPrice;
  }

  public submitForm() {
    const formValues = this.createAlertForm.value;

    const request: CreateAlertRequest = {
      financialAssetId: formValues.ticker.id,
      targetPrice: formValues.targetPrice,
    };

    this.alertsService.createAlert(request).subscribe((result) => {
      if (result.success) {
        this.createAlertForm.reset();
        this.tickerInput.nativeElement.focus();
      } else {
        console.log(result.errors);
      }
    });
  }

  public clearTickerInput(): void {
    this.createAlertForm.controls['ticker'].setValue(null);
  }

  public clearPriceInput(): void {
    this.createAlertForm.controls['targetPrice'].setValue('');
  }

  public displayAssetTicker(asset: FinancialAssetDto): string {
    return asset && asset.ticker ? asset.ticker : '';
  }
}
