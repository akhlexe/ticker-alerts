import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { NotificationService } from '../../../../core/services/notification/notification.service';
import { SearchTickerComponent } from '../../../../shared/components/search-ticker/search-ticker.component';
import { Result } from '../../../../shared/models/result.models';
import { FinancialAssetsService } from '../../../../shared/services/financial-asset/financial-assets.service';
import { FinancialAssetDto } from '../../../../shared/services/financial-asset/models/financial-asset.model';
import { AlertsService } from '../../services/alerts.service';
import { AlertMessages } from './models/alerts-messages.model';
import { CreateAlertRequest } from './models/create-alert.models';

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
    SearchTickerComponent
  ],
  templateUrl: './create-alert.component.html',
  styleUrl: './create-alert.component.css',
  providers: [AlertsService, FinancialAssetsService],
})
export class CreateAlertComponent implements OnInit {
  @Output() alertCreated: EventEmitter<void> = new EventEmitter<void>();
  @ViewChild(SearchTickerComponent) searchTickerComponent!: SearchTickerComponent;

  createAlertForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private alertsService: AlertsService,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.createAlertForm = this.formBuilder.group({
      ticker: [null, [Validators.required]],
      targetPrice: ['', [Validators.required]],
    });
  }

  public isCreateAlertFormValid() {
    return this.createAlertForm.valid;
  }

  public submitForm() {
    const formValues = this.createAlertForm.value;

    const request: CreateAlertRequest = {
      financialAssetId: formValues.ticker.id,
      targetPrice: formValues.targetPrice,
    };

    this.alertsService.createAlert(request).subscribe({
      next: (r) => this.handleResponse(r),
      error: () => this.notificationService.showError(AlertMessages.ERROR_MESSAGE, AlertMessages.CREATE_ALERT_TITLE)
    })
  }

  private handleResponse(result: Result) {
    if (result.success) {
      this.alertCreated.emit();
      this.clearInputsAndSearchList();
      this.notificationService.showSuccess(AlertMessages.SUCCESS_MESSAGE, AlertMessages.CREATE_ALERT_TITLE);
    } else {
      this.notificationService.showError(result.errors.join(' '), AlertMessages.CREATE_ALERT_TITLE);
    }
  }

  public clearInputsAndSearchList() {
    this.createAlertForm.reset();
    this.cleanErrors();
    this.searchTickerComponent.clearTickerInput();
  }

  private cleanErrors() {
    Object.keys(this.createAlertForm.controls).forEach(controlName => {
      const control = this.createAlertForm.get(controlName);
      if (control) {
        control.setErrors(null);
      }
    });
  }

  public clearPriceInput(): void {
    this.createAlertForm.controls['targetPrice'].setValue('');
  }

  public onTickerSelected(asset: FinancialAssetDto) {
    this.createAlertForm.controls['ticker'].setValue(asset);
  }
}
