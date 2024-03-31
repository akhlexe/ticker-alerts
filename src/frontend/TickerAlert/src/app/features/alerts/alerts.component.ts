import { Component, OnInit } from '@angular/core';
import { Alert } from './models/alert.model';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-alerts',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.css',
})
export class AlertsComponent implements OnInit {
  public displayedColumns: string[] = [
    'tickerName',
    'targetPrice',
    'actualPrice',
    'difference',
    'state',
  ];
  public alerts: Alert[] = [];

  ngOnInit(): void {
    this.alerts = [
      {
        tickerName: 'BTC',
        targetPrice: 70000,
        actualPrice: 60000,
        difference: 10000,
        state: 'PENDING',
      },
      {
        tickerName: 'ETH',
        targetPrice: 3800,
        actualPrice: 3650,
        difference: 150,
        state: 'PENDING',
      },
    ];
  }
}
