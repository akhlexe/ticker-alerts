import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SearchTickerComponent } from '../../../../shared/components/search-ticker/search-ticker.component';
import { FinancialAssetDto } from '../../../../shared/services/financial-asset/models/financial-asset.model';
import { MatIconModule } from '@angular/material/icon';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlertsService } from '../../services/alerts.service';
import { NotificationService } from '../../../../core/services/notification/notification.service';
import { CreateAlertRequest } from '../create-alert/models/create-alert.models';
import { AlertMessages } from '../create-alert/models/alerts-messages.model';
import { Result } from '../../../../shared/models/result.models';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

const MatModules = [
  MatDialogModule,
  MatFormFieldModule,
  MatIconModule,
  MatInputModule,
  MatButtonModule,
  MatAutocompleteModule
]

@Component({
  selector: 'app-create-alert-modal',
  standalone: true,
  imports: [CommonModule, MatModules, SearchTickerComponent, ReactiveFormsModule],
  templateUrl: './create-alert-modal.component.html',
  styleUrl: './create-alert-modal.component.css'
})
export class CreateAlertModalComponent {

  @Output() alertCreated: EventEmitter<void> = new EventEmitter<void>();
  @ViewChild(SearchTickerComponent) searchTickerComponent!: SearchTickerComponent;

  createAlertForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private alertsService: AlertsService,
    private notificationService: NotificationService,
    public dialogRef: MatDialogRef<CreateAlertModalComponent>
  ) { }

  public ngOnInit(): void {
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
      error: () => this.handleError()
    });
  }

  private handleResponse(result: Result) {
    if (result.success) {
      this.alertCreated.emit();
      this.notificationService.showSuccess(AlertMessages.SUCCESS_MESSAGE, AlertMessages.CREATE_ALERT_TITLE);
    } else {
      this.notificationService.showError(result.errors.join(' '), AlertMessages.CREATE_ALERT_TITLE);
    }

    this.dialogRef.close();
  }

  private handleError(): void {
    this.notificationService.showError(AlertMessages.ERROR_MESSAGE, AlertMessages.CREATE_ALERT_TITLE);
    this.dialogRef.close();
  }

  public clearPriceInput(): void {
    this.createAlertForm.controls['targetPrice'].setValue('');
  }

  public onTickerSelected(asset: FinancialAssetDto) {
    this.createAlertForm.controls['ticker'].setValue(asset);
  }

  public onCancel(): void {
    this.dialogRef.close();
  }
}
