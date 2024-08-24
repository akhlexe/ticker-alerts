import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { BehaviorSubject, Observable, debounceTime, of, switchMap } from 'rxjs';
import { NotificationService } from '../../../../core/services/notification/notification.service';
import { AlertsService } from '../../services/alerts.service';
import { AlertMessages } from './models/alerts-messages.model';
import { CreateAlertRequest } from './models/create-alert.models';
import { FinancialAssetDto } from './models/financial-asset.model';
import { FinancialAssetsService } from './services/financial-assets.service';

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
    private assetsService: FinancialAssetsService,
    private notificationService: NotificationService
  ) { }
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
        this.notificationService.showSuccess(AlertMessages.SUCCESS_MESSAGE, AlertMessages.CREATE_ALERT_TITLE)
      } else {
        this.notificationService.showError(result.errors.join(' '), AlertMessages.CREATE_ALERT_TITLE)
      }
    }, () => this.notificationService.showError(AlertMessages.ERROR_MESSAGE, AlertMessages.CREATE_ALERT_TITLE));
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
