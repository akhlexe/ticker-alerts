import { Component, OnInit } from '@angular/core';
import { Alert } from '../../models/alert.model';
import { AlertsService } from '../../services/alerts.service';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-alerts-table',
  standalone: true,
  imports: [MatTableModule],
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
  ];
  public alerts: Alert[] = [];

  constructor(private alertsService: AlertsService) {}

  ngOnInit(): void {
    this.alertsService.getAlerts().subscribe((result) => {
      this.alerts = result;
    });
  }
}
