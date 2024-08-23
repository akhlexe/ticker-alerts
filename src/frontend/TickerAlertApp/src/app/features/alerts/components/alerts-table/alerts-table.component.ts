import { ConfirmModalData } from './../../../../shared/components/confirm-modal/models/confirm-dialog.model';
import { Component, inject, OnInit } from '@angular/core';
import { Alert } from '../../models/alert.model';
import { AlertsService } from '../../services/alerts.service';
import { MatTableModule } from '@angular/material/table';
import { CurrencyMaskPipe } from '../../../../shared/pipes/currency-mask.pipe';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AlertState, AlertStateConfig } from '../../models/alert-state.enum';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '../../../../shared/components/confirm-modal/confirm-modal.component';
import { ToastrService } from 'ngx-toastr';

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
    private toastr: ToastrService) { }

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
            this.toastr.success('Alert cancelled correctly', 'Cancel Alert');
          } else {
            this.toastr.error(cancelResult.errors.join(' '), 'Cancel Alert');
          }
        })
      }
    });
  }

  public onReceived(id: number) {
    this.alertsService.confirmReception({ id }).subscribe(result => {
      if (result.success) {
        this.getData();
        this.toastr.success("Alert completed notified.", "Alert Confirmation");
      } else {
        this.toastr.error(result.errors.join(' '), 'Alert Confirmation')
      }
    })
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
