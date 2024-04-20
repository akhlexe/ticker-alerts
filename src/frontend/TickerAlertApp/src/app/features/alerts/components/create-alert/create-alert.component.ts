import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  AfterViewInit,
  Output,
  EventEmitter,
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
import {
  BehaviorSubject,
  Observable,
  debounceTime,
  filter,
  of,
  switchMap,
} from 'rxjs';
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
  @Output() alertCreated: EventEmitter<void> = new EventEmitter<void>();

  private searchControl = new BehaviorSubject<string | null>(null);
  assetSearchList: Observable<FinancialAssetDto[]> | undefined;
  createAlertForm!: FormGroup;

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

    this.assetSearchList = this.searchControl.pipe(
      debounceTime(300),
      switchMap((value) => {
        if (value === null || value.length <= 2) {
          return of([]);
        } else {
          return this.assetsService.getFinancialAssetsByCriteria(value);
        }
      })
    );

    this.createAlertForm.get('ticker')?.valueChanges.subscribe((value) => {
      this.searchControl.next(value);
    });
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
        this.alertCreated.emit();
        this.clearInputsAndSearchList();
        this.tickerInput.nativeElement.focus();
      } else {
        console.log(result.errors);
      }
    });
  }

  public clearInputsAndSearchList() {
    this.createAlertForm.reset();
    this.searchControl.next(null);
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
