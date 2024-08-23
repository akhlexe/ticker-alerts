import { Component, inject, OnInit } from '@angular/core';
import { Alert } from '../../models/alert.model';
import { AlertsService } from '../../services/alerts.service';
import { MatTableModule } from '@angular/material/table';
import { CurrencyMaskPipe } from '../../../../shared/pipes/currency-mask.pipe';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AlertState, AlertStateConfig } from '../../models/alert-state.enum';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alerts-table',
  standalone: true,
  imports: [MatTableModule, CurrencyMaskPipe, MatButtonModule, MatIcon, CommonModule],
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

  constructor(private alertsService: AlertsService) { }

  ngOnInit(): void {
    this.getData();
  }

  public getData() {
    this.alertsService.getAlerts().subscribe((result) => {
      this.alerts = result;
    });
  }

  public onCancel(id: number) {
    console.log(id);
  }

  public getStateLabel(state: AlertState): string {
    return AlertStateConfig[state].label;
  }

  public getStateCssClass(state: AlertState): string {
    return `alert-state-common ${AlertStateConfig[state].cssClass}`;
  }
}
