import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-alerts-table',
  standalone: true,
  imports: [MatTableModule, CurrencyMaskPipe, MatButtonModule, MatIcon, CommonModule, ConfirmModalComponent],
  templateUrl: './alerts-table.component.html',
  styleUrl: './alerts-table.component.css',
  providers: [AlertsService],
})
export class AlertsTableComponent implements OnInit {

  public displayedColumns: string[] = [
    'tickerName',
    'targetPrice',
    'actualPrice',
    'difference',
    'state',
    'actions',
  ];

  public alerts: Alert[] = [];

  constructor(
    private alertsService: AlertsService,
    private dialog: MatDialog,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.getData();
  }

  public getData() {
    this.alertsService.getAlerts().subscribe((result) => {
      this.alerts = result;
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
}
