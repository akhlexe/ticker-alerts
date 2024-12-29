import { SignalRService } from './../../../../core/services/signal-r.service';
import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { NotificationService } from '../../../../core/services/notification/notification.service';
import { ConfirmModalComponent } from '../../../../shared/components/confirm-modal/confirm-modal.component';
import { CurrencyMaskPipe } from '../../../../shared/pipes/currency-mask.pipe';
import { AlertState, AlertStateConfig } from '../../models/alert-state.enum';
import { Alert } from '../../models/alert.model';
import { AlertsService } from '../../services/alerts.service';
import { ConfirmModalData } from './../../../../shared/components/confirm-modal/models/confirm-dialog.model';
import { AssetPriceUpdateDto } from '../../../../core/services/models/signalr.model';
import { BehaviorSubject, Subject, Subscription, takeUntil } from 'rxjs';

@Component({
  selector: 'app-alerts-table',
  standalone: true,
  imports: [MatTableModule, CurrencyMaskPipe, MatButtonModule, MatIcon, CommonModule, ConfirmModalComponent],
  templateUrl: './alerts-table.component.html',
  styleUrl: './alerts-table.component.css',
  providers: [AlertsService],
})
export class AlertsTableComponent implements OnInit, OnDestroy {

  public displayedColumns: string[] = [
    'tickerName',
    'targetPrice',
    'actualPrice',
    'difference',
    'state',
    'actions',
  ];

  // public alerts: Alert[] = [];

  private alertsSubject = new BehaviorSubject<Alert[]>([]);
  public alerts$ = this.alertsSubject.asObservable();

  private destroy$ = new Subject<void>();

  constructor(
    private alertsService: AlertsService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.getData();
    this.listenForPriceUpdates();
  }

  public getData() {
    this.alertsService.getAlerts().subscribe((result) => {
      this.alertsSubject.next(result);
    });
  }

  public onCancel(id: number) {

    const dialogData: ConfirmModalData = {
      title: 'Cancel Alert',
      message: 'Do you want to cancel this alert?',
      confirmText: 'Yes',
      cancelText: 'No',
    };
    const dialogRef = this.dialog.open(ConfirmModalComponent, {
      width: '300px',
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.alertsService.cancelAlert({ id }).subscribe(cancelResult => {
          if (cancelResult.success) {
            this.getData();
            this.notificationService.showSuccess('Alert cancelled correctly', 'Cancel Alert')
          } else {
            this.notificationService.showError(cancelResult.errors.join(' '), 'Cancel Alert')
          }
        }, () => this.notificationService.showError('An error has occurred. Please try again later.', 'Cancel Alert'))
      }
    });
  }

  public onReceived(id: number) {
    this.alertsService.confirmReception({ id }).subscribe(result => {
      if (result.success) {
        this.getData();
        this.notificationService.showSuccess("Alert completed notified.", 'Alert Confirmation')
      } else {
        this.notificationService.showError(result.errors.join(' '), 'Alert Confirmation')
      }
    }, () => this.notificationService.showError('An error has occurred. Please try again later.', 'Alert Confirmation'))
  }

  public getStateLabel(state: AlertState): string {
    return AlertStateConfig[state].label;
  }

  public getStateCssClass(state: AlertState): string {
    return `alert-state-common ${AlertStateConfig[state].cssClass}`;
  }

  public isCancelVisible(state: AlertState): boolean {
    return state === AlertState.PENDING;
  }

  public isReceivedVisible(state: AlertState): boolean {
    return state === AlertState.TRIGGERED || state === AlertState.NOTIFIED;
  }

  public listenForPriceUpdates(): void {
    this.signalRService.assetPriceUpdate$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (priceUpdate) => this.updatePriceInAlerts(priceUpdate),
      error: (err) => console.log('Error receiving price updates: ', err)
    });
  }

  private updatePriceInAlerts(priceUpdate: AssetPriceUpdateDto | null): void {
    if (!priceUpdate) return;

    const currentAlerts = this.alertsSubject.value;
    if (!currentAlerts) return;

    const updatedAlerts = currentAlerts.map(item =>
      item.financialAssetId === priceUpdate.financialAssetId
        ? { ...item, price: priceUpdate.newPrice, difference: priceUpdate.newPrice - item.targetPrice }
        : { ...item }
    );

    this.alertsSubject.next(updatedAlerts);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
